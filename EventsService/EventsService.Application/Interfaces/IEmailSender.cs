namespace EventsApp.EventsService.Application.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string body);
    }
}
