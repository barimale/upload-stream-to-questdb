using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UploadStream;
using UploadStreamToQuestDB.API.CustomAttributes;
using UploadStreamToQuestDB.API.Exceptions;
using UploadStreamToQuestDB.API.SwaggerFilters;
using UploadStreamToQuestDB.Application.Handlers;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Infrastructure.Services;

namespace UploadStreamToQuestDB.API.Controllers {

    [Route("api")]
    [Produces("application/json")]
    public partial class UploadController : Controller {
        private readonly IConfiguration Configuration;
        private readonly IQueryIngestionerService _queryIngestionerService;
        private readonly ILogger<UploadController> _logger;

        public UploadController(
            IConfiguration configuration,
            IQueryIngestionerService queryIngestionerService,
            ILogger<UploadController> logger) {
            Configuration = configuration;
            _queryIngestionerService = queryIngestionerService;
            _logger = logger;
        }

        [HttpPost("stream")]
        [MultipartFormData]
        [DisableFormModelBinding]
        [FileUploadOperation.FileContentType]
        public async Task<IActionResult> ControllerStream() {
            _logger.LogInformation("Controller upload stream starts.");
            if (!Request.Headers.ContainsKey("X-SessionId") || string.IsNullOrEmpty(Request.Headers["X-SessionId"]))
                throw new XSessionIdException();

            FileModelsInput files = new FileModelsInput() {
                SessionId = Request.Headers["X-SessionId"],
                FilePath = Path.Join(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString())
            };
            _logger.LogInformation($"X-SessionId is equal to {files.SessionId}");
            _logger.LogInformation($"FilePath is equal to {files.FilePath}");

            var uploader = new UploadHandler(this);
            var extension = new ExtensionHandler(Configuration);
            var antivirus = new AntivirusHandler(Configuration);
            var db = new DataIngestionerHandler(Configuration, _queryIngestionerService);
            var diskCleanUp = new DiskCleanUpHandler();

            _logger.LogInformation($"Handler is defined.");
            uploader
                .HandleNext(extension)
                .HandleNext(antivirus)
                .HandleNext(db)
                .HandleNext(diskCleanUp);

            _logger.LogInformation($"Handler starts.");
            await uploader.Handle(files);
            _logger.LogInformation($"Handler is executed.");

            if (!ModelState.IsValid)
                return BadRequest();
            _logger.LogInformation($"Model is valid.");

            _logger.LogInformation("Controller upload stream is ended.");
            return Ok(new {
                files.SessionId,
                files.FilePath,
                Files = files.Select(x => {
                    var state = x.GetState();

                    return new {
                        state,
                        x.file.Name,
                        x.file.FileName,
                        x.file.ContentDisposition,
                        x.file.ContentType,
                        x.file.Length
                    };
                })
            });
        }
    }
}
