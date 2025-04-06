using Microsoft.Extensions.Configuration;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class ExtensionHandler : AbstractHandler, IExtensionHandler {
        private readonly IConfiguration configuration;
        public ExtensionHandler(
            IConfiguration configuration) {
            this.configuration = configuration;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            var ext = configuration["AllowedExtension"];
            if (string.IsNullOrEmpty(ext)) {
                throw new Exception("AllowedExtension ext cannot be null or empty.");
            }

            Parallel.ForEach(files, file => {
                Execute(file, ext);
            });

            return base.Handle(files);
        }

        private void Execute(FileModel input, string ext) {
            try {
                string extension = Path.GetExtension(input.File.FileName);
                if (!extension.EndsWith(ext)) {
                    input.State.Add(FileModelState.EXTENSION_NOT_OK);
                } else {
                    input.State.Add(FileModelState.EXTENSION_OK);
                }
            } catch (Exception) {
                input.State.Add(FileModelState.EXTENSION_NOT_OK);
            }

        }
    }
}
