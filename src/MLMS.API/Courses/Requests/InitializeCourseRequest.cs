namespace MLMS.API.Courses.Requests;

public class InitializeCourseRequest
{
    public string Name { get; set; }
    
    public double ExpectedTimeToFinishHours { get; set; }
    
    public int? ExpirationMonths { get; set; }
}