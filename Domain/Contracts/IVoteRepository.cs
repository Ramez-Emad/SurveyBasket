using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts;
public interface IVoteRepository : IGenericRepository<Vote>
{
    Task<bool> UserHasVotedAsync(string userId, int pollId, CancellationToken cancellationToken = default);

    Task<IEnumerable<TResult>> GetGroupedAsync<TKey, TResult>(

        ISpecifications<Vote> specification,

        Expression<Func<Vote, TKey>> groupBySelector,

        Expression<Func<IGrouping<TKey, Vote>, TResult>> resultSelector,

        CancellationToken cancellationToken = default
        );

}
