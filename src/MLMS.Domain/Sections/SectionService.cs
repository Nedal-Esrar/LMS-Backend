using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Courses;

namespace MLMS.Domain.Sections;

public class SectionService(
    ISectionRepository sectionRepository,
    ICourseRepository courseRepository) : ISectionService
{
    private readonly SectionValidator _sectionValidator = new();
    
    public async Task<ErrorOr<Section>> CreateAsync(Section section)
    {
        var validationResult = await _sectionValidator.ValidateAsync(section);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        if (!await courseRepository.ExistsByIdAsync(section.CourseId))
        {
            return CourseErrors.NotFound;
        }
        
        if (await sectionRepository.ExistsByTitleAsync(section.Title))
        {
            return SectionErrors.NameAlreadyExists;
        }
        
        section.Index = await sectionRepository.GetMaxIndexByCourseIdAsync(section.CourseId) + 1;
        
        return await sectionRepository.CreateAsync(section);
    }

    public async Task<ErrorOr<None>> UpdateAsync(long id, Section section)
    {
        var validationResult = await _sectionValidator.ValidateAsync(section);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        var existingSection = await sectionRepository.GetByIdAsync(id);
        
        if (existingSection is null)
        {
            return SectionErrors.NotFound;
        }
        
        if (existingSection.CourseId != section.CourseId)
        {
            // handle later.
        }
        
        if (!await courseRepository.ExistsByIdAsync(section.CourseId))
        {
            return CourseErrors.NotFound;
        }
        
        if (section.Title != existingSection.Title && await sectionRepository.ExistsByTitleAsync(section.Title))
        {
            return SectionErrors.NameAlreadyExists;
        }
        
        section.Index = await sectionRepository.GetMaxIndexByCourseIdAsync(section.CourseId) + 1;
        
        await sectionRepository.UpdateAsync(section);

        return None.Value;
    }

    public async Task<ErrorOr<Section>> GetByIdAsync(long courseId, long id)
    {
        if (!await sectionRepository.ExistsAsync(courseId, id))
        {
            return SectionErrors.NotFound;
        }
        
        return await sectionRepository.GetByIdAsync(id)!;
    }

    public async Task<ErrorOr<None>> DeleteAsync(long courseId, long id)
    {
        if (!await sectionRepository.ExistsAsync(courseId, id))
        {
            return SectionErrors.NotFound;
        }
        
        await sectionRepository.DeleteAsync(id);

        return None.Value;
    }
}