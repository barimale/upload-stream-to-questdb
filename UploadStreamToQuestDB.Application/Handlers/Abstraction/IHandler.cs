using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    public interface IHandler {
        IHandler HandleNext(IHandler handler);

        Task<object> Handle(FileModelsInput files);
    }
}
