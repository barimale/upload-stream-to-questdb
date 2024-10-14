using System.Collections.Concurrent;

namespace UploadStreamToQuestDB.Domain {
    public class FileModelsInput : ConcurrentBag<FileModel> {
        public required string SessionId;
        public required string FilePath;
    }
}
