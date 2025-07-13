using Domain.Entities;
namespace Domain.Contracts;
public interface IPollRepository : IGenericRepository<Poll>
{
    Task<bool> TitleExistsAsync(string title, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> IsPollAvailable(int pollId, CancellationToken cancellationToken = default);

}

