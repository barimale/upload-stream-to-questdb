using System.Collections.Concurrent;

namespace File.Api.Controllers {
        public class FileModelsInput : ConcurrentBag<FileModel> {
            public required string SessionId;
            public required string FilePath;
        }
}
