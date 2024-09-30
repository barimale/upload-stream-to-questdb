using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DiskCleanUpHandler : AbstractHandler {

        public DiskCleanUpHandler() {
        }
        public override async Task<object> Handle(FileModels files) {
            Parallel.ForEach(files, file => {
                Execute(files, file);
            });

            return base.Handle(files);
        }

        private void Execute(FileModels files, FileModel file) {
            try {
                System.IO.File.Delete(Path.Join(files.FilePath, file.file.FileName));
                file.State.Add(FileModelState.DISK_CLEANUP);
            } catch (Exception) {
                file.State.Add(FileModelState.DISK_CLEANUP_FAILED);
            }
        }
    }
}
