using System;
using System.IO;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class AntivirusHandler : AbstractHandler {
        public AntivirusHandler() {
        }
        public override object Handle(FileModels files) {
            try {
                // antivirus WINdows only
                // put bool switch on off here
                var scanner = new AntiVirus.Scanner();
                foreach (var file in files) {
                    var result = scanner.ScanAndClean(Path.Join(files.FilePath, file.file.FileName));
                    if (result != AntiVirus.ScanResult.VirusNotFound)
                        throw new Exception();
                }

                files.State = FileModelState.ANTIVIRUS_OK;

            } catch (Exception) {

                throw;
            }

            return base.Handle(files);
        }
    }
}
