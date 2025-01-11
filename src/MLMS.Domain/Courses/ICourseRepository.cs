using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using Sieve.Models;

namespace MLMS.Domain.Courses;

public interface ICourseRepository
{
    Task<Course> CreateAsync(Course course);
    
    Task<bool> ExistsByNameAsync(string courseName);
    
    Task<Course?> GetDetailedByIdAsync(long id);
    
    Task<bool> ExistsByIdAsync(long id);
    
    Task<bool> IsCreatedByUserAsync(long id, int userId);
    
    Task<PaginatedList<Course>> GetAsync(SieveModel sieveModel);
    
    Task<PaginatedList<Course>> GetAssignedByUserId(int userId, SieveModel sieveModel);
    
    Task<PaginatedList<Course>> GetCreatedByUserId(int userId, SieveModel sieveModel);
    
    Task<Course?> GetByIdAsync(long id);
    
    Task UpdateAsync(Course course);
    
    Task DeleteAsync(long id);
    
    Task<List<long>> GetExamIdsByIdAsync(long id);
}