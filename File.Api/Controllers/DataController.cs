using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questdb.Net;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadStreamToQuestDB.Domain.Utilities;

namespace UploadStreamToQuestDB.API.Controllers {
    [Route("api")]
    [Produces("application/json")]
    public class DataController : Controller {
        public class Result {
            public string StationId { get; set; }
            public double QN { get; set; }
            public double PP_10 { get; set; }
            public double TT_10 { get; set; }
            public double TM5_10 { get; set; }
            public double RF_10 { get; set; }
            public double TD_10 { get; set; }
            public DateTime Timestamp { get; set; }

        }

        [HttpGet("data/get")]
        public async Task<IActionResult> GetData([FromHeader(Name = "X-SessionId")] string sessionId, [AsParameters] PaginationRequest request) {
            var query = BuildQuery(request, sessionId);
            QuestDBClient client = new QuestDBClient("http://127.0.0.1");
            var queryApi = client.GetQueryApi();
            var dataModel = await queryApi.QueryEnumerableAsync<Result>(query);

            return Ok(new {
                firstDate = dataModel.FirstOrDefault()?.Timestamp,
                lastDate = dataModel.LastOrDefault()?.Timestamp,
                count = dataModel.ToList().Count,
                sessionId,
                results = dataModel
            });
        }

        private string BuildQuery(PaginationRequest request, string sessionId) {
            var query = new QueryBuilder()
                .WithSessionId(sessionId)
                .WithDateRange(request.StartDate, request.EndDate)
                .WithPageIndex(request.PageIndex)
                .WithPageCount(request.PageSize)
                .Build();

            return query;
        }
    }

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
