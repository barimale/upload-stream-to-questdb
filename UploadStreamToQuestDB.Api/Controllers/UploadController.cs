using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UploadStream;
using UploadStreamToQuestDB.API.CustomAttributes;
using UploadStreamToQuestDB.API.SwaggerFilters;
using UploadStreamToQuestDB.Application.Handlers;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.API.Controllers {

    [Route("api")]
    [Produces("application/json")]
    public partial class UploadController : Controller {
        private readonly IConfiguration Configuration;
        public UploadController(
            IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpPost("stream")]
        [MultipartFormData]
        [DisableFormModelBinding]
        [FileUploadOperation.FileContentType]
        public async Task<IActionResult> ControllerStream() {

            if (!Request.Headers.ContainsKey("X-SessionId") || string.IsNullOrEmpty(Request.Headers["X-SessionId"]))
                throw new Exception(
                    "SessionId needs to be added to headers. It cannot be empty");

            FileModels files = new FileModels();
            files.SessionId = Request.Headers["X-SessionId"];
            files.FilePath = Path.Join(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            var uploader = new UploadHandler(this);
            var extension = new ExtensionHandler(this, Configuration);
            var antivirus = new AntivirusHandler();
            var db = new DBIngestionerHandler();
            var diskCleanUp = new DiskCleanUpHandler();

            uploader
                .ContinueWith(extension)
                .ContinueWith(antivirus)
                .ContinueWith(db)
                .ContinueWith(diskCleanUp);

            // each table is unique having the name equals to sessionId(guid)

            await uploader.Handle(files);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(new {
                files.SessionId,
                files.FilePath,
                OperationStatus = files.State.ToString(),
                Files = files.Select(x => new {
                    x.file.Name,
                    x.file.FileName,
                    x.file.ContentDisposition,
                    x.file.ContentType,
                    x.file.Length
                })
            });
        }
    }
}
