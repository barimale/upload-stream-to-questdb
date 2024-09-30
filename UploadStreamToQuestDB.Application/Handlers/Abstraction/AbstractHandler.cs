using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    // The default chaining behavior can be implemented inside a base handler
    // class.
    public abstract class AbstractHandler : IHandler {
        private IHandler _nextHandler;

        public IHandler ContinueWith(IHandler handler) {
            _nextHandler = handler;

            // Returning a handler from here will let us link handlers in a
            // convenient way like this:
            // monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }

        public virtual Task<object> Handle(FileModels files) {
            if (_nextHandler != null) {
                return _nextHandler.Handle(files);
            } else {
                return null;
            }
        }
    }
}
