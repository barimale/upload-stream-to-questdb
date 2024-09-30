namespace UploadStreamToQuestDB.Infrastructure.Model {
    public enum FileModelState {
        UPLOADED = 0,
        EXTENSION_OK,
        EXTENSION_NOT_OK,
        ANTIVIRUS_OK,
        ANTIVIRUS_NOT_OK,
        INGESTION_READY,
        DISK_CLEANUP,
        DB_DELETED
    }
}
