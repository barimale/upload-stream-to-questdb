using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers.Abstraction {
    public interface IHandler {
        IHandler ContinueWith(IHandler handler);

        object Handle(FileModels files);
    }
}
