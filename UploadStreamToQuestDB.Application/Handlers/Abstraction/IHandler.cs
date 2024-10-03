using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    public interface IHandler {
        IHandler ContinueWith(IHandler handler);

        Task<object> Handle(FileModelsInput files);
    }
}
