using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;
public interface IQuestionRepository : IGenericRepository<Question>
{ 
    Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, CancellationToken cancellationToken = default);

    Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, int questionId, CancellationToken cancellationToken = default);
}
