using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Files;
using MLMS.Infrastructure.Common;
using File = MLMS.Domain.Files.File;

namespace MLMS.Infrastructure.Files;

public class FileRepository(LmsDbContext context) : IFileRepository
{
    public async Task CreateAsync(File file)
    {
        context.Add(file);
        
        await context.SaveChangesAsync();
    }

    public async Task<File?> GetByIdAsync(Guid id)
    {
        return await context.Files.FirstOrDefaultAsync(f => f.Id == id);
    }
}