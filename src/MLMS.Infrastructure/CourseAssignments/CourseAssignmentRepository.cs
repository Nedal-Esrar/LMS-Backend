using Microsoft.EntityFrameworkCore;
using MLMS.Domain.CourseAssignments;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.CourseAssignments;

public class CourseAssignmentRepository(LmsDbContext context) : ICourseAssignmentRepository
{
    public async Task<List<CourseAssignment>> GetByCourseIdAsync(long courseId)
    {
        return await context.CourseAssignments
            .Where(x => x.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<CourseAssignment>> GetByDepartmentAndMajorIdsAsync(int departmentId, int majorId)
    {
        return context.CourseAssignments
            .Where(x => x.DepartmentId == departmentId && x.MajorId == majorId)
            .AsNoTracking()
            .ToListAsync();
    }
}