using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Database;

namespace Infrastructure {
    public class FileContextFactory : IDesignTimeDbContextFactory<FileDbContext> {
        public FileDbContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<FileDbContext>();
            optionsBuilder.UseSqlServer("Data Source=MATEUSZ;Initial Catalog=UploadStream;TrustServerCertificate=True;Integrated Security=True;");

            return new FileDbContext(optionsBuilder.Options);
        }
    }
}
