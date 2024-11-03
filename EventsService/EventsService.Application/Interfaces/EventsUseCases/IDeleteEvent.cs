

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IDeleteEvent
    {
        Task ExecuteAsync(int id);
    }
}
