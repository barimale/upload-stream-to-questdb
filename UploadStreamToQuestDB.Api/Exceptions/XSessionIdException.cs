using System;

namespace UploadStreamToQuestDB.API.Exceptions {
    public class XSessionIdException: Exception {

        private const string MESSAGE = "X-SessionId needs to be added to headers. It cannot be empty";
        public XSessionIdException()
            : base(MESSAGE) {
            // intentionally left blank
        }
    }
}
