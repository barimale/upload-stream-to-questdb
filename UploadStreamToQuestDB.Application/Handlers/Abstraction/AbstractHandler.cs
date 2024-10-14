using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    public abstract class AbstractHandler : IHandler {
        private IHandler _nextHandler;

        public IHandler HandleNext(IHandler handler) {
            _nextHandler = handler;

            return handler;
        }

        public virtual Task<object> Handle(FileModelsInput files) {
            if (_nextHandler != null) {
                return _nextHandler.Handle(files);
            } else {
                return null;
            }
        }
    }
}
