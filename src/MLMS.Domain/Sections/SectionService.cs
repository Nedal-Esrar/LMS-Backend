using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Courses;

namespace MLMS.Domain.Sections;

public class SectionService(
    ISectionRepository sectionRepository,
    ICourseService courseService) : ISectionService
{
    private readonly SectionValidator _sectionValidator = new();
    
    public async Task<ErrorOr<Section>> CreateAsync(Section section)
    {
        var validationResult = await _sectionValidator.ValidateAsync(section);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }

        var courseExistenceResult = await courseService.CheckExistenceAsync(section.CourseId);

        if (courseExistenceResult.IsError)
        {
            return courseExistenceResult.Errors;
        }
        
        if (await sectionRepository.ExistsByTitleAsync(section.CourseId, section.Title))
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
        
        var courseExistenceResult = await courseService.CheckExistenceAsync(section.CourseId);

        if (courseExistenceResult.IsError)
        {
            return courseExistenceResult.Errors;
        }
        
        if (section.Title != existingSection.Title && await sectionRepository.ExistsByTitleAsync(section.CourseId, section.Title))
        {
            return SectionErrors.NameAlreadyExists;
        }

        existingSection.MapForUpdate(section);
        
        await sectionRepository.UpdateAsync(existingSection);

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

    public async Task<ErrorOr<None>> CheckExistenceAsync(long id)
    {
        return await sectionRepository.ExistsAsync(id) ? 
            None.Value : 
            SectionErrors.NotFound;
    }
}