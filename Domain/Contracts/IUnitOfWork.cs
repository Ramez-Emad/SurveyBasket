namespace Domain.Contracts;
public interface IUnitOfWork
{
    IPollRepository PollRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IVoteRepository VoteRepository { get; }
    IVoteAnswerRepository VoteAnswerRepository { get; }

    IUserRepository UserRepository { get; }
    IRoleClaimRepository RoleClaimRepository { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
