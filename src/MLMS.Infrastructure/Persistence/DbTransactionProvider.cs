using System.Data;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Interfaces;

namespace MLMS.Infrastructure.Persistence;

public class DbTransactionProvider(LmsDbContext context) : IDbTransactionProvider
{
    public async Task ExecuteInTransactionAsync(Func<Task> action, 
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var trx = await context.Database.BeginTransactionAsync(isolationLevel);

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