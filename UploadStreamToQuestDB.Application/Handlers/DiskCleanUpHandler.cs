using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DiskCleanUpHandler : AbstractHandler {

        public DiskCleanUpHandler() {
        }
        public override async Task<object> Handle(FileModels files) {
            try {
                foreach (var file in files) {
                    try {
                        System.IO.File.Delete(Path.Join(files.FilePath, file.file.FileName));
                        file.State = FileModelState.DISK_CLEANUP;
                    } catch (Exception) {
                        continue;
                    }
                }
            } catch (Exception) {

                throw;
            }

            return base.Handle(files);
        }
    }
}
