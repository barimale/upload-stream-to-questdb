using System.Collections.Concurrent;

namespace File.Api.Controllers {
        public class FileModelsInput : ConcurrentBag<FileModel> {
            public string SessionId;
            public string FilePath;
        }
}
