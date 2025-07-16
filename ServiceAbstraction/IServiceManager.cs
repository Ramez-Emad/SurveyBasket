namespace ServiceAbstraction;
public interface IServiceManager
{
    IPollService PollService { get; }
    IAuthService AuthService { get; }
    IQuestionService QuestionService { get; }
    IVoteService VoteService { get; }

    IUserService UserService { get; }

}
