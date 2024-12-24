namespace MLMS.Domain.CourseAssignments;

public interface ICourseAssignmentRepository
{
    Task<List<CourseAssignment>> GetByCourseIdAsync(long courseId, bool includeMajor = false);
    
    Task<List<CourseAssignment>> GetByMajorId(int majorId);
}