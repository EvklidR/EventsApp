using EventsService.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface ICreateEvent
    {
        Task<EventDto> ExecuteAsync(CreateEventDto eventDto, string? imageFile);
    }
}
