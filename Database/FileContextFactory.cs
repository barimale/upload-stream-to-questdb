using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Database;

namespace Infrastructure {
    public class FileContextFactory : IDesignTimeDbContextFactory<FileDbContext> {
        public FileDbContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<FileDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity1;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new FileDbContext(optionsBuilder.Options);
        }
    }
}
