using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DiskCleanUpHandler : AbstractHandler {

        public DiskCleanUpHandler() {
            // intentionally left blank
        }
        public override async Task<object> Handle(FileModelsInput files) {
            Parallel.ForEach(files, file => {
                Execute(files, file);
            });

            return base.Handle(files);
        }

        private void Execute(FileModelsInput files, FileModel file) {
            try {
                System.IO.File.Delete(Path.Join(files.FilePath, file.file.FileName));
                file.State.Add(FileModelState.DISK_CLEANUP);
            } catch (Exception) {
                file.State.Add(FileModelState.DISK_CLEANUP_FAILED);
            }
        }
    }
}
