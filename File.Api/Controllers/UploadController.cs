using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database;
using File.Api.Handlers;
using File.Api.SwaggerFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UploadStream;

namespace File.Api.Controllers {

    [Route("api")]
    [Produces("application/json")]
    public partial class UploadController : Controller {
        private readonly IConfiguration Configuration;
        private readonly IRepository repository;
        public UploadController(
            IConfiguration configuration,
            IRepository repository) {
            this.Configuration = configuration;
            this.repository = repository;
        }

        [HttpPost("stream")]
        [DisableFormModelBinding]
        [FileUploadOperation.FileContentType]
        public async Task<IActionResult> ControllerStream() {

            if (!this.Request.Headers.ContainsKey("X-SessionId") || string.IsNullOrEmpty(this.Request.Headers["X-SessionId"]))
                throw new Exception(
                    "SessionId needs to be added to headers. It cannot be empty");

            FileModels files = new FileModels();
            files.SessionId = this.Request.Headers["X-SessionId"];
            files.FilePath = Path.Join(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            var uploader = new UploadHandler(this, repository);
            var extension = new ExtensionHandler(this, Configuration, repository);
            var antivirus = new AntivirusHandler(repository);
            var db = new DBIngestionerHandler(repository);
            var diskCleanUp = new DiskCleanUpHandler(repository);

            uploader
                .ContinueWith(extension)
                .ContinueWith(antivirus)
                .ContinueWith(db)
                .ContinueWith(diskCleanUp);
// filedeletehandler here or background worker and do it after retention time 24 hours together with questdb clean up 
            await uploader.Handle(files);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(new {
                SessionId = files.SessionId,
                FilePath = files.FilePath,
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
