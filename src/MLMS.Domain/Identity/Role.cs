using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Identity;

public class Role : EntityBase<int>
{
    public string Name { get; set; }
}