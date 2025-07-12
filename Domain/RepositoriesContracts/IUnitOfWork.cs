using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoriesContracts;
public interface IUnitOfWork
{
    IPollRepository PollRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
