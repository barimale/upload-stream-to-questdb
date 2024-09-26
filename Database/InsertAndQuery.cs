using Domain.Utilities;
using Newtonsoft.Json.Linq;
using Questdb.Net;
using QuestDB;

namespace Database {
    public class InsertAndQuery {
        public async Task Execute(CsvFile<Foo> file, string sessionId) {
            using var sender = Sender.New("http::addr=localhost:9000;username=admin;password=quest;auto_flush=on;auto_flush_rows=80000;");
            await CreateTableIfNotExists();
            await SoftDeleteRetentionData();
            sender.Transaction("measurements");
            try {
                foreach (var p in file.records) {
                    var parsedDate = DateTimeUtility.yyyyMMddHHmmToDate(p.MessDatum);

                    await sender
                          .Symbol("sessionId", sessionId)
                          .Column("stationId", p.StationId)
                          .Column("QN", p.QN)
                          .Column("PP_10", p.PP10)
                          .Column("TT_10", p.TT10)
                          .Column("TM5_10", p.TMS10)
                          .Column("RF_10", p.RF10)
                          .Column("TD_10", p.TD10)
                          .Column("created_at", DateTime.UtcNow)
                          .Column("isDeleted", false)
                          .AtAsync(parsedDate);
                }

                await sender.CommitAsync();
            } catch (Exception) {
                sender.Rollback();
                throw;
            }
        }

        private static async Task CreateTableIfNotExists() {
            QuestDBClient client = new QuestDBClient("http://127.0.0.1");
            var queryApi = client.GetQueryApi();
            // Create table if not exists
            await queryApi.QueryRawAsync("CREATE TABLE IF NOT EXISTS 'measurements' ( sessionId SYMBOL capacity 256 CACHE, stationId LONG,  QN DOUBLE,  PP_10 DOUBLE,  TT_10 DOUBLE,  TM5_10 DOUBLE,  RF_10 DOUBLE, TD_10 DOUBLE,isDeleted BOOLEAN, created_at TIMESTAMP,  timestamp TIMESTAMP) timestamp (timestamp) PARTITION BY HOUR WAL;");
        }

        private static async Task SoftDeleteRetentionData() {
            QuestDBClient client = new QuestDBClient("http://127.0.0.1");
            var queryApi = client.GetQueryApi();
            var query = $"UPDATE 'measurements' SET isDeleted = true WHERE created_at < '{DateTimeUtility.DateToQuestDbDateString(DateTime.UtcNow.AddDays(-1).ToString("yyyyMMddHHmm"))}'";
            await queryApi.QueryRawAsync(query);
        }
    }
}
