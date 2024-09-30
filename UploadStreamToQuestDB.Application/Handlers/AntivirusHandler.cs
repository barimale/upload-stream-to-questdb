using Microsoft.Extensions.Configuration;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class AntivirusHandler : AbstractHandler {
        private readonly IConfiguration configuration;

        public AntivirusHandler(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public override async Task<object> Handle(FileModels files) {
            try {
                bool isStepActive = bool.Parse(configuration["AntivirusActive"]);
                if (isStepActive == false)
                    return base.Handle(files);

                var scanner = new AntiVirus.Scanner();
                Parallel.ForEach(files.Where(p => p.State.Contains(FileModelState.EXTENSION_OK)), async file => {
                    var result = scanner.ScanAndClean(Path.Join(files.FilePath, file.file.FileName));
                    if (result != AntiVirus.ScanResult.VirusNotFound) {
                        file.State.Add(FileModelState.ANTIVIRUS_NOT_OK);
                    } else {
                        file.State.Add(FileModelState.ANTIVIRUS_OK);
                    }
                });
            } catch (Exception) {

                throw;
            }

            return base.Handle(files);
        }
    }
}
