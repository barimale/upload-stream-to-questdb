namespace UploadStreamToQuestDB.Domain {
    public partial class DataController {
        public class WeatherDataResult {
            public string StationId { get; set; }
            public double QN { get; set; }
            public double PP_10 { get; set; }
            public double TT_10 { get; set; }
            public double TM5_10 { get; set; }
            public double RF_10 { get; set; }
            public double TD_10 { get; set; }
            public DateTime Timestamp { get; set; }

        }
    }
}
