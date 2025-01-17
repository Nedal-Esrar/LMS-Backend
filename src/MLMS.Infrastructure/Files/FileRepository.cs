using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Files;
using MLMS.Infrastructure.Persistence;
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

    public async Task DeleteAsync(Guid id)
    {
        var fileModel = await context.Files.FindAsync(id);

        if (fileModel is null)
        {
            return;
        }
        
        context.Files.Remove(fileModel);

        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Files.AnyAsync(f => f.Id == id);
    }
}