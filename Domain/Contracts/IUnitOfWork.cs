using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;
public interface IUnitOfWork
{
    IPollRepository PollRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IVoteRepository VoteRepository { get; }
    IVoteAnswerRepository VoteAnswerRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
