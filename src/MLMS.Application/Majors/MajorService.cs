using ErrorOr;
using FluentValidation;
using MLMS.Application.Common;
using MLMS.Domain.Entities;
using MLMS.Domain.Majors;

namespace MLMS.Application.Majors;

public class MajorService(
    IValidator<Major> majorValidator,
    IMajorRepository majorRepository) : IMajorService
{
    public async Task<ErrorOr<Major>> CreateAsync(Major major)
    {
        var validationResult = await majorValidator.ValidateAsync(major);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }

        if (await majorRepository.ExistsByNameAsync(major.DepartmentId, major.Name))
        {
            return MajorErrors.NameAlreadyExists;
        }
        
        var createdMajor = await majorRepository.CreateAsync(major);

        return createdMajor;
    }

    public async Task<ErrorOr<None>> DeleteAsync(int departmentId, int id) 
    {
        if (!await majorRepository.ExistsAsync(departmentId, id))
        {
            return MajorErrors.NotFound;
        }
        
        await majorRepository.DeleteAsync(departmentId, id);

        return None.Value;
    }

    public async Task<ErrorOr<Major>> GetByIdAsync(int departmentId, int id)
    {
        var major = await majorRepository.GetByIdAsync(departmentId, id);

        return major is null ? MajorErrors.NotFound : major;
    }

    public async Task<ErrorOr<List<Major>>> GetByDepartmentAsync(int departmentId)
    {
        return await majorRepository.GetByDepartmentAsync(departmentId);
    }
}