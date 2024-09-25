using Microsoft.AspNetCore.Http;

namespace File.Api.Controllers {
    public partial class UploadController {
        public class FileModel {
            public IFormFile file;
            public string FilePath;
        }
    }
}
