using Domain.Utilities;
using QuestDB;

namespace Database {
    public class InsertAndQuery {
        public async Task Execute(CsvFile<Foo> file, string sessionId) {
            using var sender = Sender.New("http::addr=localhost:9000;username=admin;password=quest;auto_flush=on;auto_flush_rows=80000;");
            sender.Transaction("measurements");
            try {
                foreach(var p in file.records) {
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
                          .AtAsync(parsedDate);
                }

                await sender.CommitAsync();
            } catch (Exception) {
                sender.Rollback();
                throw;
            }
        }
    }
}
