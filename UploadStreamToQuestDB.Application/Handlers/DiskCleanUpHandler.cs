using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DiskCleanUpHandler : AbstractHandler {

        public DiskCleanUpHandler() {
        }
        public override async Task<object> Handle(FileModels files) {
            try {
                Parallel.ForEach(files, file => {
                    try {
                        System.IO.File.Delete(Path.Join(files.FilePath, file.file.FileName));
                        file.State.Add(FileModelState.DISK_CLEANUP);
                    } catch (Exception) {
                    }
                });
            } catch (Exception) {

                throw;
            }

            return base.Handle(files);
        }
    }
}
