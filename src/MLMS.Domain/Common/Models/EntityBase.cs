namespace MLMS.Domain.Common.Models;

public class EntityBase<TId> where TId: notnull
{
    public TId Id { get; set; }
}