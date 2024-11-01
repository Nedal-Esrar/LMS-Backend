using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Departments;
using MLMS.Domain.Entities;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Common;

public class LmsDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<Department> Departments { get; set; }
    
    public DbSet<Major> Majors { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public LmsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        var cascadeDeleteFKs = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior != DeleteBehavior.Restrict);

        foreach (var fk in cascadeDeleteFKs)
            fk.DeleteBehavior = DeleteBehavior.NoAction;
    }
}