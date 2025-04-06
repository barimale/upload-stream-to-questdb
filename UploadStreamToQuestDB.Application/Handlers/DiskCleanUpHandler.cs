using Microsoft.Extensions.Logging;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DiskCleanUpHandler : AbstractHandler, IDiskCleanUpHandler {

        private readonly ILogger<DiskCleanUpHandler> _logger;

        public DiskCleanUpHandler(ILogger<DiskCleanUpHandler> logger) {
            this._logger = logger;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            Parallel.ForEach(files, file => {
                Execute(files, file);
            });

            return base.Handle(files);
        }

        private void Execute(FileModelsInput files, FileModel file) {
            try {
                File.Delete(Path.Join(files.FilePath, file.File.FileName));
                file.State.Add(FileModelState.DISK_CLEANUP);
            } catch (Exception ex) {
                _logger.LogError($"Error deleting file {file.File.FileName}: {ex.Message}");
                file.State.Add(FileModelState.DISK_CLEANUP_FAILED);
            }
        }
    }
}
