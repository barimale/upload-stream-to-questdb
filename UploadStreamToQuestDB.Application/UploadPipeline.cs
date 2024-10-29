using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadStreamToQuestDB.Application.Handlers;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application {
    public class UploadPipeline : IUploadPipeline {
        private readonly IUploadHandler uploadHandler;
        private readonly IExtensionHandler extensionHandler;
        private readonly IAntivirusHandler antivirusHandler;
        private readonly IDataIngestionerHandler dataIngestionerHandler;
        private readonly IDiskCleanUpHandler diskCleanUpHandler;
        private readonly ILogger<UploadPipeline> logger;

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

        public void Initialize(Controller controller) {
            uploadHandler.SetController(controller);
            logger.LogTrace("Controller for UploadHandler is set.");
            logger.LogTrace("Start configure pipeline.");
            uploadHandler
               .HandleNext(extensionHandler)
               .HandleNext(antivirusHandler)
               .HandleNext(dataIngestionerHandler)
               .HandleNext(diskCleanUpHandler);
            logger.LogTrace("Pipeline configuration is ended.");
        }

        public async Task Run(FileModelsInput files) {
            logger.LogTrace("Start handling.");
            await uploadHandler.Handle(files);
            logger.LogTrace("Handling is ended.");
        }
    }
}
