using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Database.UploadController;

namespace Database {
    public class FileDbContext : DbContext {

        public DbSet<FileModelEntry> FileModelEntries { get; set; }

        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
