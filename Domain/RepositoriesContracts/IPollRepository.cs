using Domain.Entities;
namespace Domain.RepositoriesContracts;
public interface IPollRepository : IGenericRepository<Poll>
{
    Task<bool> TitleExistsAsync(string title, int? excludeId = null, CancellationToken cancellationToken = default);

}

