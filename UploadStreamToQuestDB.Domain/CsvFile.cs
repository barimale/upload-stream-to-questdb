using CsvHelper.Configuration.Attributes;

namespace UploadStreamToQuestDB.Domain {
    public class CsvFile<T> {
        public List<T> Records { get; private set; } = new List<T>();
    }
    /// <summary>
    /// Represents weather data for Germany.
    /// </summary>
    public class WeatherGermany {
        /// <summary>
        /// Gets or sets the station ID.
        /// </summary>
        [Name("STATIONS_ID")]
        public int StationId { get; set; }

        /// <summary>
        /// Gets or sets the measurement date.
        /// </summary>
        [Name("MESS_DATUM")]
        public string MessDatum { get; set; }

        /// <summary>
        /// Gets or sets the quality control level.
        /// </summary>
        [Name("QN")]
        public double QN { get; set; }

        /// <summary>
        /// Gets or sets the precipitation amount.
        /// </summary>
        [Name("PP_10")]
        public double PP10 { get; set; }

        /// <summary>
        /// Gets or sets the air temperature.
        /// </summary>
        [Name("TT_10")]
        public double TT10 { get; set; }

        /// <summary>
        /// Gets or sets the soil temperature at 5 cm depth.
        /// </summary>
        [Name("TM5_10")]
        public double TMS10 { get; set; }

        /// <summary>
        /// Gets or sets the relative humidity.
        /// </summary>
        [Name("RF_10")]
        public double RF10 { get; set; }

        /// <summary>
        /// Gets or sets the dew point temperature.
        /// </summary>
        [Name("TD_10")]
        public double TD10 { get; set; }
    }
}
