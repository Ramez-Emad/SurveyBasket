using Domain.Contracts;
using Persistence.Data;


namespace Persistence.Repositories;
public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    #region PollRepository
    private IPollRepository? _pollRepository;
    public IPollRepository PollRepository => _pollRepository ??= new PollRepository(dbContext);
    #endregion

    #region QuestionRepository
    private IQuestionRepository? _questionRepository;
    public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(dbContext);
    #endregion

    #region VoteRepository
    private IVoteRepository? _voteRepository;
    public IVoteRepository VoteRepository => _voteRepository ??= new VoteRepository(dbContext);
    #endregion

    #region VoteAnswerRepository
    private IVoteAnswerRepository? _voteAnswerRepository;
    public IVoteAnswerRepository VoteAnswerRepository => _voteAnswerRepository ??= new VoteAnswerRepository(dbContext);
    #endregion


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}


/*
{

- Database => data Access layer -> GetAll DomainModel

- Service = > DomainModel => DTO

}*/