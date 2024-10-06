using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UploadStream;
using UploadStreamToQuestDB.API.CustomAttributes;
using UploadStreamToQuestDB.API.Exceptions;
using UploadStreamToQuestDB.API.SwaggerFilters;
using UploadStreamToQuestDB.Application.Handlers;
using UploadStreamToQuestDB.Infrastructure.Services;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.API.Controllers {

    [Route("api")]
    [Produces("application/json")]
    public partial class UploadController : Controller {
        private readonly IConfiguration Configuration;
        private readonly IQueryIngestionerService _queryIngestionerService;

        public UploadController(
            IConfiguration configuration,
            IQueryIngestionerService queryIngestionerService) {
            Configuration = configuration;
            _queryIngestionerService = queryIngestionerService;
        }

        [HttpPost("stream")]
        [MultipartFormData]
        [DisableFormModelBinding]
        [FileUploadOperation.FileContentType]
        public async Task<IActionResult> ControllerStream() {

            if (!Request.Headers.ContainsKey("X-SessionId") || string.IsNullOrEmpty(Request.Headers["X-SessionId"]))
                throw new XSessionIdException();

            FileModelsInput files = new FileModelsInput() {
                SessionId = Request.Headers["X-SessionId"],
                FilePath = Path.Join(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString())
            };

            var uploader = new UploadHandler(this);
            var extension = new ExtensionHandler(Configuration);
            var antivirus = new AntivirusHandler(Configuration);
            var db = new DataIngestionerHandler(Configuration, _queryIngestionerService);
            var diskCleanUp = new DiskCleanUpHandler();

            uploader
                .HandleNext(extension)
                .HandleNext(antivirus)
                .HandleNext(db)
                .HandleNext(diskCleanUp);

            await uploader.Handle(files);

            if (!ModelState.IsValid)
                return BadRequest();

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
