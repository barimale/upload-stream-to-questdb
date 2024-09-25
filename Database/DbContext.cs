using Microsoft.EntityFrameworkCore;
using static Database.UploadController;

namespace Database {
    public class DbContext {
        public DbSet<FileModelEntry> FileModelEntries { get; set; }

    }
}
