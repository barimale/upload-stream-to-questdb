using Microsoft.AspNetCore.Http;
using UploadStreamToQuestDB.Infrastructure.Model;

namespace File.Api.Controllers {
    public partial class UploadController {
        public class FileModel {
            public IFormFile file;
            public string FilePath;
            public List<FileModelState> State = new List<FileModelState>();
        }
    }
}
