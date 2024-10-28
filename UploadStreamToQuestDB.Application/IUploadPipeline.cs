using Microsoft.AspNetCore.Mvc;
using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application {
    public interface IUploadPipeline {
        void Initialize(Controller controller);
        Task Run(FileModelsInput files);
    }
}
