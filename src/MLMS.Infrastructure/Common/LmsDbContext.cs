using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.KeyPerFile;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using MLMS.Domain.Majors;
using MLMS.Domain.Notifications;
using MLMS.Infrastructure.Departments;
using MLMS.Infrastructure.Files;
using MLMS.Infrastructure.Identity;
using MLMS.Infrastructure.Identity.Models;
using MLMS.Infrastructure.Majors;
using File = MLMS.Domain.Files.File;

namespace MLMS.Infrastructure.Common;

public class LmsDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<Department> Departments { get; set; }
    
    public DbSet<Major> Majors { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<File> Files { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    public LmsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new DepartmentConfiguration());
        builder.ApplyConfiguration(new MajorConfiguration());
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new FileConfiguration());
        
        // builder.
    }
}