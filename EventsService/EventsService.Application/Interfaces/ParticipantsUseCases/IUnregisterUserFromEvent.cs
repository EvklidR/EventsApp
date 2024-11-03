namespace EventsService.Application.Interfaces.ParticipantsUseCases
{
    public interface IUnregisterUserFromEvent
    {
        Task ExecuteAsync(int eventId, int userId);
    }
}
