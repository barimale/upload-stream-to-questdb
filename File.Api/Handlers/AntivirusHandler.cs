using Database;
using File.Api.Handlers.Abstraction;
using Infrastructure.Entries;
using System;
using System.IO;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    class AntivirusHandler : AbstractHandler {
        private readonly IRepository repository;

        public AntivirusHandler(IRepository repository) {
            this.repository = repository;
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
            } finally {
                // DB state | healthy | virus
            }

            return base.Handle(files);
        }
    }
}
