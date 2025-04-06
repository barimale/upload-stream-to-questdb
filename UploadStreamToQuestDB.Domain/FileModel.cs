using Microsoft.AspNetCore.Http;

namespace UploadStreamToQuestDB.Domain {
    /// <summary>
    /// Represents a file model with its state and path.
    /// </summary>
    public class FileModel {
        /// <summary>
        /// Gets the uploaded file.
        /// </summary>
        public IFormFile File { get; private set; }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the list of file model states.
        /// </summary>
        public IList<FileModelState> State { get; private set; } = new List<FileModelState>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel"/> class.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <param name="filePath">The file path.</param>
        public FileModel(IFormFile file, string filePath) {
            File = file;
            FilePath = filePath;
        }

        /// <summary>
        /// Gets the state of the file as a comma-separated string.
        /// </summary>
        /// <returns>A comma-separated string representing the file state.</returns>
        public string GetState() => string.Join(",", State.Select(p => p.ToString()));
    }
}
