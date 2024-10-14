using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    public interface IHandler {
        IHandler HandleNext(IHandler handler);

        Task<object> Handle(FileModelsInput files);
    }
}
