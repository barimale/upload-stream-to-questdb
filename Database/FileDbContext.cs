using Microsoft.EntityFrameworkCore;
using static Database.UploadController;

namespace Database {
    public class FileDbContext : DbContext {

        public DbSet<FileModelEntry> FileModelEntries { get; set; }

        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options) {
            Database.EnsureCreated();
        }
    }
}
