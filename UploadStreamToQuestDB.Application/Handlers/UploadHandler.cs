using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadStream;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class UploadHandler : AbstractHandler, IUploadHandler {
        private readonly ILogger<UploadHandler> _logger;
        private Controller controller;

        public UploadHandler(ILogger<UploadHandler> logger) {
            this._logger = logger;
        }

        public void SetController(Controller controller) {
            this.controller = controller;
        }
        public async override Task<object> Handle(FileModelsInput files) {
            await controller.Request.StreamFilesModel(async x => {
                try {
                    using (var stream = x.OpenReadStream()) {
                        if (!Directory.Exists(files.FilePath)) {
                            Directory.CreateDirectory(files.FilePath);
                        }

                        using (var fileStream = new FileStream(
                            Path.Join(files.FilePath, x.FileName),
                            FileMode.Create)) {
                            await stream.CopyToAsync(fileStream);
                        }

                        var entry = new FileModel(x, Path.Join(files.FilePath, x.FileName));
                        entry.State.Add(FileModelState.UPLOADED);
                        files.Add(entry);
                    }
                } catch (Exception ex) {
                    _logger.LogError($"Error uploading file {x.FileName}: {ex.Message}");
                }
            });

            return base.Handle(files);
        }
    }
}
