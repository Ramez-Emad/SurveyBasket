using Domain.RepositoriesContracts;
using Persistence.Data;


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
