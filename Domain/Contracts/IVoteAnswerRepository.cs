using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;
public interface IVoteAnswerRepository : IGenericRepository<VoteAnswer>
{
    Task<IEnumerable<TResult>> GetGroupedAsync<TKey, TResult>(

       ISpecifications<VoteAnswer> specification,

       Expression<Func<VoteAnswer, TKey>> groupBySelector,

       Expression<Func<IGrouping<TKey, VoteAnswer>, TResult>> resultSelector,

       CancellationToken cancellationToken = default
       );
}
