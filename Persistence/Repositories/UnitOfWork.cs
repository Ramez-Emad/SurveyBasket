using Domain.Contracts;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;
public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    #region PollRepository
    private IPollRepository? _pollRepository;
    public IPollRepository PollRepository => _pollRepository ??= new PollRepository(dbContext);
    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}
