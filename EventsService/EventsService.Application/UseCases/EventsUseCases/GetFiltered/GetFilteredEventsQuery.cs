using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Enums;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetFilteredEventsQuery : IRequest<IEnumerable<EventDto>?>
    {
        public GetFilteredEventsQuery(EventFilterDto eventFilterDto) 
        {
            Filter = eventFilterDto;
        }
        public EventFilterDto Filter { get; set; }
    }
}