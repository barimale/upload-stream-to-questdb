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
            uploadHandler.SetUploadHandler(controller);
            logger.LogInformation("Controller for UploadHandler is set.");
            logger.LogInformation("Start configure pipeline.");
            uploadHandler
               .HandleNext(extensionHandler)
               .HandleNext(antivirusHandler)
               .HandleNext(dataIngestionerHandler)
               .HandleNext(diskCleanUpHandler);
            logger.LogInformation("Pipeline configuration is ended.");
        }

        public async Task Run(FileModelsInput files) {
            logger.LogInformation("Start handling.");
            await uploadHandler.Handle(files);
            logger.LogInformation("Handling is ended.");
        }
    }
}
