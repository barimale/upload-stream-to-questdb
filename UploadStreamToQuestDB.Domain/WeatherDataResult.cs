namespace UploadStreamToQuestDB.Domain {
        public class WeatherDataResult {
            public required string StationId { get; init; }
            public required double QN { get; init; }
            public required double PP_10 { get; init; }
            public required double TT_10 { get; init; }
            public required double TM5_10 { get; init; }
            public required double RF_10 { get; init; }
            public required double TD_10 { get; init; }
            public required DateTime Timestamp { get; init; }

        }
}
