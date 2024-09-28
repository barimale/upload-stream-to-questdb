namespace UploadStreamToQuestDB.Infrastructure.Model {
    public enum FileModelState {
        UPLOADED = 0,
        EXTENSION_OK,
        ANTIVIRUS_OK,
        INGESTION_READY,
        DISK_CLEANUP,
        DB_DELETED
    }
}
