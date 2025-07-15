using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Service.Authentication;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

 

}
