namespace MLMS.Domain.CourseAssignments;

public interface ICourseAssignmentRepository
{
    Task<List<CourseAssignment>> GetByCourseIdAsync(long courseId, bool includeMajorAssignments = false);
    
    Task<List<CourseAssignment>> GetByMajorIdAsync(int majorId);
    
    Task CreateAsync(List<CourseAssignment> assignments);
    
    Task DeleteAsync(List<CourseAssignment> assignments);
}