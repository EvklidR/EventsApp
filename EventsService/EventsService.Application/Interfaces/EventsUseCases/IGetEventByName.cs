using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetEventByName
    {
        EventDto Execute(string name);
    }
}
