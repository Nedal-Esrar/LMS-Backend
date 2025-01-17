using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = MLMS.Domain.Files.File;

namespace MLMS.Infrastructure.Files;

public class FileDbConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("File");
        
        builder.HasKey(f => f.Id);
    }
}