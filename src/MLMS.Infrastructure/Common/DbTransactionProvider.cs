using System.Data;
using MLMS.Domain.Common.Interfaces;

namespace MLMS.Infrastructure.Common;

public class DbTransactionProvider(LmsDbContext context) : IDbTransactionProvider
{
    public async Task ExecuteInTransactionAsync(Func<Task> action, 
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var trx = await context.Database.BeginTransactionAsync();

        try
        {
            await action();

            await trx.CommitAsync();
        }
        catch
        {
            await trx.RollbackAsync();

            throw;
        }
    }
}