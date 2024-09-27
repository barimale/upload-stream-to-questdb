using File.Api.Handlers.Abstraction;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using UploadStream;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    public class UploadHandler : AbstractHandler {
        private readonly Controller controller;

        public UploadHandler(Controller controller) {
            this.controller = controller;
        }
        public async Task Handle(FileModels files) {

            try {
                await controller.Request.StreamFilesModel(async x => {
                    using (var stream = x.OpenReadStream()) {
                        if (!Directory.Exists(files.FilePath)) {
                            Directory.CreateDirectory(files.FilePath);
                        }
                        using (var fileStream = new FileStream(
                            Path.Join(files.FilePath, x.FileName),
                            FileMode.Create)) {
                            await stream.CopyToAsync(fileStream);
                        }
                        var entry = new FileModel() {
                            file = x,
                            FilePath = Path.Join(files.FilePath, x.FileName),
                            State = FileModelState.UPLOADED
                        };
                        files.Add(entry);
                    }
                });

                files.State = FileModelState.UPLOADED;
            } catch (System.Exception) {

                throw;
            }

            base.Handle(files);
        }
    }
}
