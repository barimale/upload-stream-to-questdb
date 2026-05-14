using Npgsql;
using QuestDB;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Domain.Utilities;
using UploadStreamToQuestDB.Infrastructure.Model.Connection;

namespace UploadStreamToQuestDB.Infrastructure.Services {
    /// <summary>
    /// Service for ingesting queries into QuestDB.
    /// </summary>
    public class QueryIngestionerService : IQueryIngestionerService {
        private readonly string address;
        private readonly string port;
        private readonly string settings;
        private readonly int npgslPort;
        private readonly string npgslDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryIngestionerService"/> class.
        /// </summary>
        /// <param name="address">The address of the QuestDB server.</param>
        /// <param name="port">The port of the QuestDB server.</param>
        /// <param name="settings">The settings for the QuestDB connection.</param>
        /// <param name="npgslPort">The port of the QuestDB server for npgsl.</param>
        /// <param name="npgslDatabase">The database name for the npgsl connection.</param>
        public QueryIngestionerService(string address, string port, string settings, int npgslPort, string npgslDatabase) {
            this.address = address;
            this.port = port;
            this.settings = settings;
            this.npgslPort = npgslPort;
            this.npgslDatabase = npgslDatabase;
        }

        /// <summary>
        /// Executes the ingestion of a CSV file into QuestDB.
        /// </summary>
        /// <param name="file">The CSV file containing weather data.</param>
        /// <param name="sessionId">The session ID for the name of the table.</param>
        public void Execute(CsvFile<WeatherGermany> file, string sessionId) {
            using var sender = Sender.New($"http::addr={this.address}:{this.port};{this.settings}");
            sender.Transaction(sessionId);
            try {
                foreach (var p in file.Records) {
                    var parsedDate = DateTimeUtility.yyyyMMddHHmmToDate(p.MessDatum);

                    sender
                     .Symbol("stationId", p.StationId.ToString())
                     .Column("QN", p.QN)
                     .Column("PP_10", p.PP10)
                     .Column("TT_10", p.TT10)
                     .Column("TM5_10", p.TMS10)
                     .Column("RF_10", p.RF10)
                     .Column("TD_10", p.TD10)
                     .At(parsedDate);
                }

                sender.Commit();
            } catch (Exception ex) {
                sender.Rollback();
                throw;
            }
        }

        public async Task CreateTable(string sessionId, CancellationToken ct = default) {
            var password = this.settings.Split(";").FirstOrDefault(s => s.StartsWith("password="))?.Split("=").LastOrDefault() ?? "quest";
            var username = this.settings.Split(";").FirstOrDefault(s => s.StartsWith("username="))?.Split("=").LastOrDefault() ?? "admin";

            var connectionFactory = new NpgsqlConnectionFactory(
                host: this.address,
                port: this.npgslPort,
                username: username,
                password: password,
                database: this.npgslDatabase
            );

            await using var conn = (NpgsqlConnection)await connectionFactory.CreateConnectionAsync(ct);

            await using var cmd = conn.CreateCommand();

            cmd.CommandText = $@"
                CREATE TABLE IF NOT EXISTS {sessionId} (
                    timestamp TIMESTAMP,
                    stationId SYMBOL CAPACITY 500000,
                    QN DOUBLE,
                    PP_10 DOUBLE,
                    TT_10 DOUBLE,
                    TM5_10 DOUBLE,
                    RF_10 DOUBLE,
                    TD_10 DOUBLE
                ) timestamp (timestamp)
                  PARTITION BY DAY
                  WAL
                "; // DEDUP UPSERT KEYS(timestamp, stationId);


            await cmd.ExecuteNonQueryAsync(ct);
        }
    }
}
