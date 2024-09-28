using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    public interface IHandler {
        IHandler ContinueWith(IHandler handler);

        object Handle(FileModels files);
    }
}
