using System.Collections.Generic;
using UploadStreamToQuestDB.Infrastructure.Model;

namespace File.Api.Controllers {
    public partial class UploadController {
        public class FileModels : List<FileModel> {
            public string SessionId;
            public string FilePath;
            public FileModelState State;
        }
    }
}
