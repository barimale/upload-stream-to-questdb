using AntiVirus;
using Microsoft.Extensions.Configuration;
using nClam;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class AntivirusnClamHandler : AbstractHandler, IAntivirusHandler {
        private readonly IConfiguration configuration;

        public AntivirusnClamHandler(IConfiguration configuration) {
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
                var clam = new ClamClient("localhost", 3310);
                // or var clam = new ClamClient(IPAddress.Parse("127.0.0.1"), 3310);
                var scanResult = clam.ScanFileOnServerAsync(Path.Join(files.FilePath, file.file.FileName)).Result;  //any file you would like!
                if (scanResult.Result != ClamScanResults.Clean) {
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
