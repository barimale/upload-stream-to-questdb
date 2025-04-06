using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class ExtensionHandler : AbstractHandler, IExtensionHandler {
        private readonly IConfiguration configuration;
        private readonly ILogger<ExtensionHandler> _logger;

        public ExtensionHandler(
            IConfiguration configuration,
            ILogger<ExtensionHandler> logger) {
            this.configuration = configuration;
            this._logger = logger;
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
            } catch (Exception ex) {
                _logger.LogError(ex, $"Error processing file {input.File.FileName}: {ex.Message}");
                input.State.Add(FileModelState.EXTENSION_NOT_OK);
            }

        }
    }
}
