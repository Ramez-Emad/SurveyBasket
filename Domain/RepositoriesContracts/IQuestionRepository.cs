using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoriesContracts;
public interface IQuestionRepository : IGenericRepository<Question>
{

    /// <summary>
    /// Checks whether a question with the specified content already exists for the given poll,
    /// optionally excluding a specific question by ID.
    /// </summary>

    Task<bool> ExistsAsync(string content, int pollId, int? questionId ,  CancellationToken cancellationToken = default);

    Task<IEnumerable<Question>> GetQuestionsByPollIdAsync(int pollId, CancellationToken cancellationToken = default);

}
