using System.Collections.Generic;
using System.Collections.Concurrent;
using UploadStreamToQuestDB.Infrastructure.Model;

namespace File.Api.Controllers {
    public partial class UploadController {
        public class FileModels : ConcurrentBag<FileModel> {
            public string SessionId;
            public string FilePath;
        }
    }
}
