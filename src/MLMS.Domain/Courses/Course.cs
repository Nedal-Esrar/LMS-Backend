using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.SectionParts;
using MLMS.Domain.Sections;
using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;

namespace MLMS.Domain.Courses;

public class Course : EntityBase<long>
{
    public string Name { get; set; }
    
    public double ExpectedTimeToFinishHours { get; set; }
    
    public int? ExpirationMonths { get; set; }
    
    public List<Section> Sections { get; set; } = [];
    
    public List<UserCourse> UsersCourses { get; set; } = [];
    
    public List<CourseAssignment> Assignments { get; set; } = [];

    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime? UpdatedAtUtc { get; set; }

    public int? CreatedById { get; set; }
    
    public User? CreatedBy { get; set; }
    
    public bool IsDeleted { get; set; }

    public double Progress
    {
        get
        {
            if (Sections.Count == 0)
            {
                return 0.0;
            }
            
            var totalSectionParts = Sections.Sum(s => s.SectionParts.Count);

            if (totalSectionParts == 0)
            {
                return 0.0;
            }

            var doneSectionParts = Sections.Sum(s => s.SectionParts.Count(sp =>
                (sp.UserSectionPartStatuses.FirstOrDefault()?.Status ?? SectionPartStatus.NotViewed) ==
                SectionPartStatus.Done));
            
            return 1.0 * doneSectionParts / totalSectionParts;
        }
    }
}