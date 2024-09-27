using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Infrastructure.Entries.UploadController;

namespace Christmas.Secret.Gifter.Infrastructure.EntityConfigurations {
    public class EventEntryConfiguration : IEntityTypeConfiguration<FileModelEntry>
    {
        public void Configure(EntityTypeBuilder<FileModelEntry> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
