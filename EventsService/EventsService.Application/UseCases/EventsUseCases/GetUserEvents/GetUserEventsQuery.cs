using EventsService.Application.DTOs;
using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetUserEventsQuery : IRequest<IEnumerable<EventDto>>
    {
        public GetUserEventsQuery(int userId) 
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}
