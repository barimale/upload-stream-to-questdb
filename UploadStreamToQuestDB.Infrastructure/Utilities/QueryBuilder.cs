using System.Text;

namespace UploadStreamToQuestDB.Domain.Utilities.QueryUtilities {
    /// <summary>
    /// Builder class for constructing SQL queries for QuestDB.
    /// </summary>
    public class QueryBuilder {
        private const string start =
            "select StationId,QN,PP_10,TT_10,TM5_10,RF_10,TD_10,timestamp from ";

        private int? PageIndex;
        private int? PageCount;
        private string SessionId = string.Empty;
        private string StartDate = string.Empty;
        private string EndDate = string.Empty;

        public QueryBuilder WithDateRange(string? start, string? end) {
            if(string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                return this;

            StartDate = DateTimeUtility.DateToQuestDbDateString(start);
            EndDate = DateTimeUtility.DateToQuestDbDateString(end);
            return this;
        }

        public QueryBuilder WithSessionId(string value) {
            SessionId = value;
            return this;
        }

        public QueryBuilder WithPageIndex(int? value) {
            PageIndex = value;
            return this;
        }

        public QueryBuilder WithPageCount(int? value) {
            PageCount = value;
            return this;
        }

        /// <summary>
        /// Builds the SQL query string.
        /// </summary>
        /// <returns>The constructed SQL query string.</returns>
        public string Build() {
            var whereUsed = false;
            var builder = new StringBuilder(start);
            builder.Append($"'{SessionId}'");

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate)) {
                builder.Append((whereUsed ? " AND " : " WHERE ") + $" timestamp BETWEEN '{StartDate}' AND '{EndDate}'");
                whereUsed = true;
            }

            if (PageIndex != null && PageCount != null) {
                builder.Append(" LIMIT " + PageIndex * PageCount + ", " + (PageIndex * PageCount + PageCount));
            }
                return builder.ToString();
        }
    }
}
