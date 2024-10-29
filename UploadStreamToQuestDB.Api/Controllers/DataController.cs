using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Questdb.Net;
using System.Linq;
using System.Threading.Tasks;
using UploadStreamToQuestDB.API.Exceptions;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Domain.Utilities.QueryUtilities;

namespace UploadStreamToQuestDB.API.Controllers {
    [Route("api")]
    [Produces("application/json")]
    public class DataController : Controller {
        private readonly IQuestDBClient questDbClient;
        private readonly ILogger<DataController> _logger;

        public DataController(IQuestDBClient questDbClient,
            ILogger<DataController> logger) {
            this.questDbClient = questDbClient;
            this._logger = logger;
        }

        [HttpGet("data/get")]
        public async Task<IActionResult> GetData([FromHeader(Name = "X-SessionId")] string sessionId, [AsParameters] PaginationRequest request) {
            _logger.LogTrace("Controller get-data starts.");
            if (string.IsNullOrEmpty(sessionId))
                throw new XSessionIdException();

            _logger.LogTrace($"X-SessionId is equal to {sessionId}.");
            var query = BuildQuery(request, sessionId);
            _logger.LogTrace($"Query is equal to {query}.");

            var queryApi = questDbClient.GetQueryApi();
            _logger.LogTrace("QueryApi is created.");
            _logger.LogTrace("Data is downloading.");
            var dataModel = await queryApi.QueryEnumerableAsync<WeatherDataResult>(query);
            _logger.LogTrace("Data is downloaded.");

            _logger.LogTrace("Controller get-data is ended.");
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
