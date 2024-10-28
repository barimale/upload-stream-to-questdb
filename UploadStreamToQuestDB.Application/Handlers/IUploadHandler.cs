using Microsoft.AspNetCore.Mvc;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;

namespace UploadStreamToQuestDB.Application.Handlers {
    public interface IUploadHandler: IHandler {
       public void SetUploadHandler(Controller controller);
    }
}
