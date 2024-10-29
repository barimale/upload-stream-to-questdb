using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadStream;
using UploadStreamToQuestDB.API.CustomAttributes;
using UploadStreamToQuestDB.API.Exceptions;
using UploadStreamToQuestDB.API.SwaggerFilters;
using UploadStreamToQuestDB.Application;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.API.Controllers {

    [Route("api")]
    [Produces("application/json")]
    public class UploadController : Controller {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadPipeline _pipeline;
        public UploadController(
            ILogger<UploadController> logger,
            IUploadPipeline pipeline) {
            _logger = logger;
            _pipeline = pipeline;
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

            _logger.LogInformation($"Pipeline is initializing.");
            _pipeline.Initialize(this);
            _logger.LogInformation($"Pipeline is initialized.");

            _logger.LogInformation($"Pipeline starts.");
            await _pipeline.Run(files);
            _logger.LogInformation($"Pipeline is executed.");

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
