using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Questdb.Net;
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
        private readonly ILogger<DataController> _logger;

        public DataController(IQuestDBClient questDbClient,
            ILogger<DataController> logger) {
            this.questDbClient = questDbClient;
            this._logger = logger;
        }

        [HttpGet("data/get")]
        public async Task<IActionResult> GetData([FromHeader(Name = "X-SessionId")] string sessionId, [AsParameters] PaginationRequest request) {
            _logger.LogInformation("Controller get-data is started.");
            if (string.IsNullOrEmpty(sessionId))
                throw new XSessionIdException();

            _logger.LogInformation($"X-SessionId is equal to {sessionId}.");
            var query = BuildQuery(request, sessionId);
            _logger.LogInformation($"Query is equal to {query}.");

            _logger.LogInformation("QueryApi is created.");
            var queryApi = questDbClient.GetQueryApi();
            _logger.LogInformation("Data is downloading.");
            var dataModel = await queryApi.QueryEnumerableAsync<WeatherDataResult>(query);
            _logger.LogInformation("Data is downloaded.");

            _logger.LogInformation("Controller get-data is ended.");
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
