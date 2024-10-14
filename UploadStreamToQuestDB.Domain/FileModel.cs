using Microsoft.AspNetCore.Http;

namespace UploadStreamToQuestDB.Domain {
    public class FileModel {
        public IFormFile file;
        public string FilePath;
        public List<FileModelState> State = new List<FileModelState>();
        public string GetState() {
            return string.Join(",", State.Select(p => p.ToString()));
        }
    }
}
