using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questdb.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using UploadStreamToQuestDB.API.Exceptions;
using UploadStreamToQuestDB.Domain.Utilities.QueryUtilities;
using static UploadStreamToQuestDB.Domain.DataController;

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
                throw new XSessionIdException();

            var query = BuildQuery(request, sessionId);
            var queryApi = questDbClient.GetQueryApi();
            var dataModel = await queryApi.QueryEnumerableAsync<WeatherDataResult>(query);

            return Ok(new {
                sessionId,
                count = dataModel.ToList().Count,
                firstDate = dataModel.FirstOrDefault()?.Timestamp,
                lastDate = dataModel.LastOrDefault()?.Timestamp,
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
