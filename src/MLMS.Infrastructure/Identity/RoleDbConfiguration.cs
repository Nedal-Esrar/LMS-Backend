using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MLMS.Infrastructure.Identity;

public class RoleDbConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        builder.ToTable("Role")
            .Ignore(r => r.ConcurrencyStamp)
            .Ignore(r => r.NormalizedName);
    }
}