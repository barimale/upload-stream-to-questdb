using Microsoft.AspNetCore.Mvc;
using UploadStreamToQuestDB.Application.Handlers;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application {
    public class UploadPipeline : IUploadPipeline {
        private readonly IUploadHandler uploadHandler;
        private readonly IExtensionHandler extensionHandler;
        private readonly IAntivirusHandler antivirusHandler;
        private readonly IDataIngestionerHandler dataIngestionerHandler;
        private readonly IDiskCleanUpHandler diskCleanUpHandler;

        public UploadPipeline(IUploadHandler uploadHandler,
            IExtensionHandler extensionHandler,
            IAntivirusHandler antivirusHandler,
            IDataIngestionerHandler dataIngestionerHandler,
            IDiskCleanUpHandler diskCleanUpHandler) {
            this.uploadHandler = uploadHandler;
            this.extensionHandler = extensionHandler;
            this.antivirusHandler = antivirusHandler;
            this.dataIngestionerHandler = dataIngestionerHandler;
            this.diskCleanUpHandler = diskCleanUpHandler;
        }

        public void Initialize(Controller controller) {
            uploadHandler.SetUploadHandler(controller);

            uploadHandler
               .HandleNext(extensionHandler)
               .HandleNext(antivirusHandler)
               .HandleNext(dataIngestionerHandler)
               .HandleNext(diskCleanUpHandler);
        }

        public async Task Run(FileModelsInput files) {
            await uploadHandler.Handle(files);
        }
    }
}
