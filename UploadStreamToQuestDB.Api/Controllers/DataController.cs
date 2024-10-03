using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Questdb.Net;
using System.Linq;
using System.Threading.Tasks;
using UploadStreamToQuestDB.Application.Model.QueryUtilities;
using static UploadStreamToQuestDB.Application.Model.DataController;

namespace UploadStreamToQuestDB.API.Controllers {
    [Route("api")]
    [Produces("application/json")]
    public partial class DataController : Controller {
        private readonly IConfiguration Configuration;
        public DataController(IConfiguration configuration) {
                this.Configuration = configuration;
        }

        [HttpGet("data/get")]
        public async Task<IActionResult> GetData([FromHeader(Name = "X-SessionId")] string sessionId, [AsParameters] PaginationRequest request) {
            var query = BuildQuery(request, sessionId);
            QuestDBClient client = new QuestDBClient("http://127.0.0.1");
            var queryApi = client.GetQueryApi();
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
