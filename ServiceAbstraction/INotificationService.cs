namespace ServiceAbstraction;

public interface INotificationService
{
    Task SendNewPollsNotification(int? pollId = null);
}