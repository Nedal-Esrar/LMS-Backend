using MLMS.Domain.Sections;

namespace MLMS.Infrastructure.Sections;

public class SectionRepository : ISectionRepository
{
    public Task<bool> ExistsAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(long courseId, long id)
    {
        throw new NotImplementedException();
    }

    public Task<Section> CreateAsync(Section section)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetMaxIndexByCourseIdAsync(long courseId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Section section)
    {
        throw new NotImplementedException();
    }

    public Task<Section?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}