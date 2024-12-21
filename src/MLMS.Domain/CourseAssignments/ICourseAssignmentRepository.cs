namespace MLMS.Domain.CourseAssignments;

public interface ICourseAssignmentRepository
{
    Task<List<CourseAssignment>> GetByCourseIdAsync(long courseId);
    
    Task<List<CourseAssignment>> GetByDepartmentAndMajorIdsAsync(int departmentId, int majorId);
}