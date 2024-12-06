using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Departments;

public class Department : EntityBase<int>
{
    public string Name { get; set; }
}