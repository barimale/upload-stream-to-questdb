using System;
using System.IO;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DiskCleanUpHandler : AbstractHandler {

        public DiskCleanUpHandler() {
        }
        public override object Handle(FileModels files) {
            try {
                // put bool switch on off here
                foreach (var file in files) {
                    try {
                        System.IO.File.Delete(Path.Join(files.FilePath, file.file.FileName));
                    } catch (Exception) {
                        continue;
                    }
                }

                files.State = FileModelState.DISK_CLEANUP;

            } catch (Exception) {

                throw;
            }

            return base.Handle(files);
        }
    }
}
