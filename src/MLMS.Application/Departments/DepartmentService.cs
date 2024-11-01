using ErrorOr;
using FluentValidation;
using MLMS.Application.Common;
using MLMS.Domain.Departments;
using MLMS.Domain.Entities;

namespace MLMS.Application.Departments;

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

    public async Task<ErrorOr<List<Department>>> GetAsync()
    {
        return await departmentRepository.GetAsync();
    }
}