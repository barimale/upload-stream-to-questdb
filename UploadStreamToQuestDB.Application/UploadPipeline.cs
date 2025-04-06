using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadStreamToQuestDB.Application.Handlers;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application {
    /// <summary>
    /// Represents the upload pipeline for handling file uploads.
    /// </summary>
    public class UploadPipeline : IUploadPipeline {
        private readonly IUploadHandler uploadHandler;
        private readonly IExtensionHandler extensionHandler;
        private readonly IAntivirusHandler antivirusHandler;
        private readonly IDataIngestionerHandler dataIngestionerHandler;
        private readonly IDiskCleanUpHandler diskCleanUpHandler;
        private readonly ILogger<UploadPipeline> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPipeline"/> class.
        /// </summary>
        /// <param name="uploadHandler">The upload handler.</param>
        /// <param name="extensionHandler">The extension handler.</param>
        /// <param name="antivirusHandler">The antivirus handler.</param>
        /// <param name="dataIngestionerHandler">The data ingestion handler.</param>
        /// <param name="diskCleanUpHandler">The disk cleanup handler.</param>
        /// <param name="logger">The logger.</param>
        public UploadPipeline(IUploadHandler uploadHandler,
            IExtensionHandler extensionHandler,
            IAntivirusHandler antivirusHandler,
            IDataIngestionerHandler dataIngestionerHandler,
            IDiskCleanUpHandler diskCleanUpHandler,
            ILogger<UploadPipeline> logger) {
            this.uploadHandler = uploadHandler;
            this.extensionHandler = extensionHandler;
            this.antivirusHandler = antivirusHandler;
            this.dataIngestionerHandler = dataIngestionerHandler;
            this.diskCleanUpHandler = diskCleanUpHandler;
            this.logger = logger;
        }

        /// <summary>
        /// Initializes the upload pipeline
        /// </summary>
        /// <param name="controller">The controller to set for the upload handler.</param>
        public void Initialize(Controller controller) {
            uploadHandler.SetController(controller);
            logger.LogTrace("Controller for UploadHandler is set.");
            logger.LogTrace("Pipeline configuration starts.");
            uploadHandler
               .SetNext(extensionHandler)
               .SetNext(antivirusHandler)
               .SetNext(dataIngestionerHandler)
               .SetNext(diskCleanUpHandler);
            logger.LogTrace("Pipeline configuration is ended.");
        }

        /// <summary>
        /// Runs the upload pipeline with the specified files.
        /// Order of handlers is specific.
        /// </summary>
        /// <param name="files">The files to process.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Run(FileModelsInput files) {
            logger.LogTrace("Start handling.");
            await uploadHandler.Handle(files);
            logger.LogTrace("Handling is ended.");
        }
    }
}
