using System.Collections.Concurrent;

namespace UploadStreamToQuestDB.Domain {
    /// <summary>
    /// Represents a collection of file models with additional session and file path information.
    /// </summary>
    public class FileModelsInput : ConcurrentBag<FileModel> {
        public required string SessionId;
        public required string FilePath;

        public IEnumerable<FileModel> ToDataIngestionHandler(bool isStepActive) {
            return this.Where(p => (
                isStepActive && p.State.Contains(FileModelState.ANTIVIRUS_OK))
                || (isStepActive == false && p.State.Contains(FileModelState.EXTENSION_OK)));
        }

        public IEnumerable<FileModel> ToAntivirusHandler() {
            return this.Where(p => p.State.Contains(FileModelState.EXTENSION_OK));
        }
    }
}
