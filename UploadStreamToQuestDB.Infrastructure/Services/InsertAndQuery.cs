using Questdb.Net;
using QuestDB;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Domain.Utilities;

namespace UploadStreamToQuestDB.Infrastructure.Services {
    public class InsertAndQuery {
        public async Task Execute(CsvFile<WeatherGermany> file, string sessionId) {
            //await CreateTableIfNotExists(sessionId);

            using var sender = Sender.New("http::addr=localhost:9000;username=admin;password=quest;auto_flush=on;auto_flush_rows=80000;");
            sender.Transaction(sessionId);
            try {
                foreach (var p in file.records) {
                    var parsedDate = DateTimeUtility.yyyyMMddHHmmToDate(p.MessDatum);
                    
                    await sender
                          .Symbol("stationId", p.StationId.ToString())
                          .Column("QN", p.QN)
                          .Column("PP_10", p.PP10)
                          .Column("TT_10", p.TT10)
                          .Column("TM5_10", p.TMS10)
                          .Column("RF_10", p.RF10)
                          .Column("TD_10", p.TD10)
                          .AtAsync(parsedDate);
                }

                await sender.CommitAsync();
            } catch (Exception) {
                sender.Rollback();
                throw;
            } finally {
                // alter index here
            }
        }

        // something wrong with the lib, connection not disposed or sth
        // rewrite as described here:
        // https://questdb.io/docs/reference/sql/overview/#questdb-web-console
        private static async Task CreateTableIfNotExists(string sessionId) {
            using (var client = new QuestDBClient("http://127.0.0.1")) {
                var queryApi = client.GetQueryApi();
                var result = await queryApi.QueryRawAsync($"CREATE TABLE IF NOT EXISTS '{sessionId}' ( stationId SYMBOL,  QN DOUBLE,  PP_10 DOUBLE,  TT_10 DOUBLE,  TM5_10 DOUBLE,  RF_10 DOUBLE, TD_10 DOUBLE, timestamp TIMESTAMP), INDEX (stationId CAPACITY 256) timestamp (timestamp) PARTITION BY HOUR WAL;");
            }
        }
    }
}
