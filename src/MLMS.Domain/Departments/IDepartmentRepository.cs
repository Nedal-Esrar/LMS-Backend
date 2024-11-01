namespace MLMS.Domain.Departments;

public interface IDepartmentRepository
{
    Task<Department> CreateAsync(Department department);
    
    Task<bool> ExistsAsync(int id);
    
    Task DeleteAsync(int id);
    
    Task<Department?> GetByIdAsync(int id);
    
    Task<List<Department>> GetAsync();
}