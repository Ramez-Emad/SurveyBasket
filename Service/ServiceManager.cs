using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Hybrid;
using Service.Authentication;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class ServiceManager(IUnitOfWork _unitOfWork , UserManager<ApplicationUser> _userManager , HybridCache hybridCache, IJwtProvider _jwtProvider) : IServiceManager
{

    private IPollService? _pollService;
    public IPollService PollService => _pollService ??= new PollService(_unitOfWork);


    private IAuthService? _authService;
    public IAuthService AuthService => _authService ??= new AuthService(_userManager , _jwtProvider);

    private IQuestionService? _questionService;
    public IQuestionService QuestionService => _questionService ??= new QuestionService(_unitOfWork , hybridCache);

    private IVoteService? _voteService;
    public IVoteService VoteService => _voteService ??= new VoteService(_unitOfWork);

 

}
