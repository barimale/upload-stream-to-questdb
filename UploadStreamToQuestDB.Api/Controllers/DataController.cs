using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Questdb.Net;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using UploadStreamToQuestDB.API.Exceptions;
using UploadStreamToQuestDB.API.Model;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Domain.Utilities.QueryUtilities;

namespace UploadStreamToQuestDB.API.Controllers {
    /// <summary>
    /// Controller for handling data-related requests.
    /// </summary>
    [Route("api")]
    [Produces("application/json")]
    public class DataController : Controller {
        private readonly IQuestDBClient questDbClient;
        private readonly ILogger<DataController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataController"/> class.
        /// </summary>
        /// <param name="questDbClient">The QuestDB client.</param>
        /// <param name="logger">The logger.</param>
        public DataController(IQuestDBClient questDbClient,
            ILogger<DataController> logger) {
            this.questDbClient = questDbClient;
            this._logger = logger;
        }

        /// <summary>
        /// Endpoint for getting data from the server.
        /// </summary>
        /// <param name="sessionId">The session ID from the header.</param>
        /// <param name="request">The pagination request parameters.</param>
        /// <returns>An <see cref="IActionResult"/> containing the data.</returns>
        [HttpGet("data/get")]
        [SwaggerOperation(Summary = "Endpoint for getting data from server.")]
        public async Task<IActionResult> GetData(
            [FromHeader(Name = "X-SessionId")] string sessionId,
            [AsParameters] PaginationRequest request) {
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

        /// <summary>
        /// Builds the SQL query string based on the pagination request and session ID.
        /// </summary>
        /// <param name="request">The pagination request parameters.</param>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The constructed SQL query string.</returns>
        private string BuildQuery(PaginationRequest request, string sessionId) {
            var query = new QueryBuilder()
                .WithSessionId(sessionId)
                .WithDateRange(request.StartDate, request.EndDate)
                .WithPageIndexAndCount(request.PageIndex, request.PageSize)
                .Build();

            return query;
        }
    }
}
