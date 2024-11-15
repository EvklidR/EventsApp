using EventsService.Application.DTOs;
using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetUserEventsCommand : IRequest<IEnumerable<EventDto>>
    {
        public GetUserEventsCommand(int userId) 
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}
