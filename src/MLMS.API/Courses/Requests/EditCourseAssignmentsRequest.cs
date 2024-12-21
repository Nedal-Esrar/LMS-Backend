namespace MLMS.API.Courses.Requests;

public class EditCourseAssignmentsRequest
{
    public List<(int DepartmentId, int MajorId)> Assignments { get; set; } = [];
}