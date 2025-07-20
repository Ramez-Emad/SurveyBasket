using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Service.Email;
using Service.Specifications;
using ServiceAbstraction;
using Shared.Abstractions.Consts;

namespace Service;
public class NotificationService(UserManager<ApplicationUser> _userManager, ILogger<NotificationService> logger, IUnitOfWork unitOfWork, IHttpContextAccessor _httpContextAccessor, IEmailSender _emailSender) : INotificationService
{

    public async Task SendNewPollsNotification(int? pollId = null)
    {

        IEnumerable<Poll> polls = [];
        var spec = new PollsForNotificationSpecification(pollId);

        if (pollId.HasValue)
        {
            var poll = await unitOfWork.PollRepository.GetAsync(spec);
        }
        else
        {
            polls = await unitOfWork.PollRepository.GetAllAsync(spec);
        }

        var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Member);

        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;


        foreach (var poll in polls)
        {
            foreach (var user in users)
            {
                var placeholders = new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                    { "{{pollTill}}", poll.Title },
                    { "{{endDate}}", poll.EndsAt.ToString() },
                    { "{{url}}", $"{origin}/polls/start/{poll.Id}" }
                };

                var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);

                logger.LogInformation($"Sending email to {user.Email} for poll {poll.Title}");
                await _emailSender.SendEmailAsync(user.Email!, $"📣 Survey Basket: New Poll - {poll.Title}", body);
            }
        }
    }
}
