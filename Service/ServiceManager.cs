using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Service.Authentication;
using ServiceAbstraction;

namespace Service;
public class ServiceManager(IUnitOfWork _unitOfWork , UserManager<ApplicationUser> _userManager , HybridCache hybridCache, IJwtProvider _jwtProvider, ILogger<AuthService> logger,
        IHttpContextAccessor _httpContextAccessor, IEmailSender emailSender, INotificationService notificationService) : IServiceManager
{

    private IPollService? _pollService;
    public IPollService PollService => _pollService ??= new PollService(_unitOfWork, notificationService);


    private IAuthService? _authService;
    public IAuthService AuthService => _authService ??= new AuthService(_userManager , _jwtProvider ,logger ,_httpContextAccessor, emailSender);


    private IQuestionService? _questionService;
    public IQuestionService QuestionService => _questionService ??= new QuestionService(_unitOfWork , hybridCache);


    private IVoteService? _voteService;
    public IVoteService VoteService => _voteService ??= new VoteService(_unitOfWork);


    private IUserService? _userService;
    public IUserService UserService => _userService ??= new UserService(_userManager);
}
