using Microsoft.Extensions.Configuration;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class ExtensionHandler : AbstractHandler {
        private readonly IConfiguration configuration;
        public ExtensionHandler(
            IConfiguration configuration) {
            this.configuration = configuration;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            var ext = configuration["AllowedExtension"];

            Parallel.ForEach(files, file => {
                Execute(file, ext);
            });

            return base.Handle(files);
        }

        private void Execute(FileModel file, string? ext) {
            try {
                string extension = Path.GetExtension(file.file.FileName);
                if (!extension.EndsWith(ext)) {
                    file.State.Add(FileModelState.EXTENSION_NOT_OK);
                } else {
                    file.State.Add(FileModelState.EXTENSION_OK);
                }
            } catch (Exception) {
                file.State.Add(FileModelState.EXTENSION_NOT_OK);
            }

        }
    }
}
