using QuestDB;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Domain.Utilities;

namespace UploadStreamToQuestDB.Infrastructure.Services {
    public class InsertAndQuery {
        private readonly string address;
        private readonly string port;
        private readonly string settings;
        public InsertAndQuery(string address, string port, string settings) {
            this.address = address;
            this.port = port;
            this.settings = settings;
        }

        public void Execute(CsvFile<WeatherGermany> file, string sessionId) {
            using var sender = Sender.New($"http::addr={this.address}:{this.port};{this.settings};");
            sender.Transaction(sessionId);
            try {
                foreach (var p in file.records) {
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
            } catch (Exception) {
                sender.Rollback();
                throw;
            }
        }
    }
}
