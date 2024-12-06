using ErrorOr;
using FluentValidation;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Departments;

public class DepartmentService(
    IValidator<Department> departmentValidator,
    IDepartmentRepository departmentRepository) : IDepartmentService
{
    public async Task<ErrorOr<Department>> CreateAsync(Department department)
    {
        var validationResult = await departmentValidator.ValidateAsync(department);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        if (await departmentRepository.ExistsByNameAsync(department.Name))
        {
            return DepartmentErrors.NameAlreadyExists;
        }
        
        var createdDepartment = await departmentRepository.CreateAsync(department);

        return createdDepartment;
    }

    public async Task<ErrorOr<None>> DeleteAsync(int id)
    {
        if (!await departmentRepository.ExistsAsync(id))
        {
            return DepartmentErrors.NotFound;
        }
        
        await departmentRepository.DeleteAsync(id);

        return None.Value;
    }

    public async Task<ErrorOr<Department>> GetByIdAsync(int id)
    {
        var department = await departmentRepository.GetByIdAsync(id);
        
        return department is null ? DepartmentErrors.NotFound : department;
    }

    public async Task<ErrorOr<PaginatedList<Department>>> GetAsync(SieveModel sieveModel)
    {
        return await departmentRepository.GetAsync(sieveModel);
    }

    public async Task<ErrorOr<None>> UpdateAsync(int id, Department toDomain)
    {
        var validationResult = await departmentValidator.ValidateAsync(toDomain);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        var department = await departmentRepository.GetByIdAsync(id);

        if (department is null)
        {
            return DepartmentErrors.NotFound;
        }
        
        department.Name = toDomain.Name;
        
        await departmentRepository.UpdateAsync(department);

        return None.Value;
    }
}