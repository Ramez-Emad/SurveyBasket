using Domain.Entities;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using Service.Authentication;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class ServiceManager(IUnitOfWork _unitOfWork , UserManager<ApplicationUser> _userManager , IJwtProvider _jwtProvider) : IServiceManager
{

    private IPollService? _pollService;
    public IPollService PollService => _pollService ??= new PollService(_unitOfWork);


    private IAuthService? _authService;
    public IAuthService AuthService => _authService ??= new AuthService(_userManager , _jwtProvider);

    private IQuestionService? _questionService;
    public IQuestionService QuestionService => _questionService ??= new QuestionService(_unitOfWork);

    private IVoteService? _voteService;
    public IVoteService VoteService => _voteService ??= new VoteService(_unitOfWork);

    private IVoteAnswerService? _voteAnswerService;
    public IVoteAnswerService VoteAnswerService => _voteAnswerService ??= new VoteAnswerService(_unitOfWork);

}
