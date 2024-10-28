using AntiVirus;
using Microsoft.Extensions.Configuration;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class AntivirusHandler : AbstractHandler, IAntivirusHandler {
        private readonly IConfiguration configuration;

        public AntivirusHandler(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            bool isStepActive = bool.Parse(configuration["AntivirusActive"]);
            if (isStepActive == false)
                return base.Handle(files);

            Parallel.ForEach(files.Where(p => p.State.Contains(FileModelState.EXTENSION_OK)), file => {
                Execute(files, file);
            });

            return base.Handle(files);
        }

        private void Execute(FileModelsInput files, FileModel file) {
            try {
                var scanner = new Scanner();
                var result = scanner.ScanAndClean(Path.Join(files.FilePath, file.file.FileName));
                if (result != ScanResult.VirusNotFound) {
                    file.State.Add(FileModelState.ANTIVIRUS_NOT_OK);
                } else {
                    file.State.Add(FileModelState.ANTIVIRUS_OK);
                }
            } catch (Exception) {
                file.State.Add(FileModelState.ANTIVIRUS_NOT_OK);
            }

        }
    }
}
