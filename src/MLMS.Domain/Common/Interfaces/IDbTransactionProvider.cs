using System.Data;

namespace MLMS.Domain.Common.Interfaces;

public interface IDbTransactionProvider
{
    Task ExecuteInTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}