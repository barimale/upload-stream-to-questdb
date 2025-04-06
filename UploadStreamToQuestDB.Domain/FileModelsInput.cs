using System.Collections.Concurrent;

namespace UploadStreamToQuestDB.Domain {
    /// <summary>
    /// Represents a collection of file models with additional session and file path information.
    /// </summary>
    public class FileModelsInput : ConcurrentBag<FileModel> {
        public required string SessionId;
        public required string FilePath;
    }
}
