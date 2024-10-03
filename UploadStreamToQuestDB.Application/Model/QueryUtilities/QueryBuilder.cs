using System.Text;
using UploadStreamToQuestDB.Domain.Utilities;

namespace UploadStreamToQuestDB.Application.Model.QueryUtilities {
    public class QueryBuilder {
        private const string start =
            "select StationId,QN,PP_10,TT_10,TM5_10,RF_10,TD_10,timestamp from ";

        private int PageIndex;
        private int PageCount;
        private string SessionId;
        private string StartDate;
        private string EndDate;

        public QueryBuilder WithDateRange(string value, string value2) {
            StartDate = DateTimeUtility.DateToQuestDbDateString(value);
            EndDate = DateTimeUtility.DateToQuestDbDateString(value2);
            return this;
        }

        public QueryBuilder WithSessionId(string value) {
            SessionId = value;
            return this;
        }

        public QueryBuilder WithPageIndex(int value) {
            PageIndex = value;
            return this;
        }

        public QueryBuilder WithPageCount(int value) {
            PageCount = value;
            return this;
        }

        public string Build() {
            var whereUsed = false;
            var builder = new StringBuilder();
            builder.Append(start);
            builder.Append($"'{SessionId}'");
            builder.Append((whereUsed ? " AND " : " WHERE ") + $" timestamp BETWEEN '{StartDate}' AND '{EndDate}'");
            if (!whereUsed)
                whereUsed = true;


            builder.Append(" LIMIT " + PageIndex * PageCount + ", " + (PageIndex * PageCount + PageCount));

            return builder.ToString();
        }
    }
}
