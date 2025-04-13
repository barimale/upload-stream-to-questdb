using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application {
    /// <summary>
    /// Defines the interface for an upload pipeline.
    /// </summary>
    public interface IUploadPipeline {
        /// <summary>
        /// Initializes the upload pipeline with the specified controller.
        /// </summary>
        /// <param name="controller">The controller to set for the upload handler.</param>
        void Initialize(Controller controller);

        /// <summary>
        /// Runs the upload pipeline with the specified files.
        /// </summary>
        /// <param name="files">The files to process.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Run(FileModelsInput files);
    }
}
