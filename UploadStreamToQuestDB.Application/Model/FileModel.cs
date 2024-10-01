using Microsoft.AspNetCore.Http;
using UploadStreamToQuestDB.Infrastructure.Model;
using System.Collections.Concurrent;

namespace File.Api.Controllers {
    public partial class UploadController {
        public class FileModel {
            public IFormFile file;
            public string FilePath;
            public ConcurrentBag<FileModelState> State = new List<FileModelState>();
       public string GetState()
       {
           return string.Join(",", this.State.Select(p => p.ToString()));
       }
        }
    }
}
