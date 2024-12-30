using Microsoft.EntityFrameworkCore;
using MLMS.Domain.CourseAssignments;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.CourseAssignments;

public class CourseAssignmentRepository(LmsDbContext context) : ICourseAssignmentRepository
{
    public async Task<List<CourseAssignment>> GetByCourseIdAsync(long courseId, bool includeMajorAssignments)
    {
        var query = context.CourseAssignments
            .Where(x => x.CourseId == courseId);
        
        if (includeMajorAssignments)
        {
            query = query.Include(x => x.Major)
                .ThenInclude(x => x.Department);
        }
        
        return await query.AsNoTracking()
            .ToListAsync();
    }

    public Task<List<CourseAssignment>> GetByMajorIdAsync(int majorId)
    {
        return context.CourseAssignments
            .Where(x => x.MajorId == majorId)
            .AsNoTracking()
            .ToListAsync();
    }
}