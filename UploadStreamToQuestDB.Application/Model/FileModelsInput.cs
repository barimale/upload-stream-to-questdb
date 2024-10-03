using System.Collections.Concurrent;

namespace File.Api.Controllers {
    public partial class UploadController {
        public class FileModelsInput : ConcurrentBag<FileModel> {
            public string SessionId;
            public string FilePath;
        }
    }
}
