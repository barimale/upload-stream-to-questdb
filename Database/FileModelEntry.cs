namespace Database {
    public partial class UploadController {
        public class FileModelEntry {
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string SessionId { get; set; }
            public FileModelState State { get; set; }
        }
    }
}
