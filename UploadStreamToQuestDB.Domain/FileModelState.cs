namespace UploadStreamToQuestDB.Infrastructure.Model {
    public enum FileModelState {
        UPLOADED = 0,
        EXTENSION_OK,
        EXTENSION_NOT_OK,
        ANTIVIRUS_OK,
        ANTIVIRUS_NOT_OK,
        INGESTION_READY,
        INGESTION_FAILED,
        DISK_CLEANUP,
        DISK_CLEANUP_FAILED
    }
}
