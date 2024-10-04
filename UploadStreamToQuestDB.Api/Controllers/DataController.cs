using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Questdb.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using UploadStreamToQuestDB.Application.Model.QueryUtilities;
using static UploadStreamToQuestDB.Application.Model.DataController;

namespace UploadStreamToQuestDB.API.Controllers {
    [Route("api")]
    [Produces("application/json")]
    public partial class DataController : Controller {
        private readonly IQuestDBClient questDbClient;
        public DataController(IQuestDBClient questDbClient) {
                this.questDbClient = questDbClient;
        }

        [HttpGet("data/get")]
        public async Task<IActionResult> GetData([FromHeader(Name = "X-SessionId")] string sessionId, [AsParameters] PaginationRequest request) {
            if (string.IsNullOrEmpty(sessionId))
                throw new Exception(
                    "X-SessionId needs to be added to headers. It cannot be empty");

            var query = BuildQuery(request, sessionId);
            var queryApi = questDbClient.GetQueryApi();
            var dataModel = await queryApi.QueryEnumerableAsync<WeatherDataResult>(query);

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
}
