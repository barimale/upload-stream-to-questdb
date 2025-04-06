using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Application.Handlers.Abstraction {
    /// <summary>
    /// Abstract base class for handling file models in a chain of responsibility pattern.
    /// </summary>
    public abstract class AbstractHandler : IHandler {
        private IHandler _nextHandler;

        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="handler">The next handler.</param>
        /// <returns>The next handler.</returns>
        public IHandler SetNext(IHandler handler) {
            _nextHandler = handler;

            return handler;
        }

        /// <summary>
        /// Handles the file models input. If the current handler cannot handle the input, it passes it to the next handler in the chain.
        /// </summary>
        /// <param name="files">The file models input.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual Task<object> Handle(FileModelsInput files) {
            if (_nextHandler != null) {
                return _nextHandler.Handle(files);
            } else {
                return null;
            }
        }
    }
}
