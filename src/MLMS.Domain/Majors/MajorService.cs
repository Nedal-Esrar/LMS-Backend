using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using Sieve.Models;

namespace MLMS.Domain.Majors;

public class MajorService(
    IMajorRepository majorRepository,
    IDepartmentRepository departmentRepository) : IMajorService
{
    private readonly MajorValidator _majorValidator = new();
    
    public async Task<ErrorOr<Major>> CreateAsync(Major major)
    {
        var validationResult = await _majorValidator.ValidateAsync(major);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        if (!await departmentRepository.ExistsAsync(major.DepartmentId)) 
        {
            return DepartmentErrors.NotFound;
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
        if (!await departmentRepository.ExistsAsync(departmentId)) 
        {
            return DepartmentErrors.NotFound;
        }
        
        if (!await majorRepository.ExistsAsync(departmentId, id))
        {
            return MajorErrors.NotFound;
        }
        
        await majorRepository.DeleteAsync(departmentId, id);

        return None.Value;
    }

    public async Task<ErrorOr<Major>> GetByIdAsync(int departmentId, int id)
    {
        if (!await departmentRepository.ExistsAsync(departmentId)) 
        {
            return DepartmentErrors.NotFound;
        }
        
        var major = await majorRepository.GetByIdAsync(departmentId, id);

        return major is null ? MajorErrors.NotFound : major;
    }

    public async Task<ErrorOr<PaginatedList<Major>>> GetByDepartmentAsync(int departmentId, SieveModel sieveModel)
    {
        if (!await departmentRepository.ExistsAsync(departmentId)) 
        {
            return DepartmentErrors.NotFound;
        }
        
        return await majorRepository.GetByDepartmentAsync(departmentId, sieveModel);
    }

    public async Task<ErrorOr<None>> UpdateAsync(int departmentId, int id, Major updatedMajor)
    {
        if (!await departmentRepository.ExistsAsync(departmentId))
        {
            return DepartmentErrors.NotFound;
        }

        var validationResult = await _majorValidator.ValidateAsync(updatedMajor);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }

        var major = await majorRepository.GetByIdAsync(departmentId, id);

        if (major is null)
        {
            return MajorErrors.NotFound;
        }
        
        if (major.Name != updatedMajor.Name && await majorRepository.ExistsByNameAsync(departmentId, updatedMajor.Name))
        {
            return MajorErrors.NameAlreadyExists;
        }

        major.Name = updatedMajor.Name;
        
        await majorRepository.UpdateAsync(major);

        return None.Value;
    }
}