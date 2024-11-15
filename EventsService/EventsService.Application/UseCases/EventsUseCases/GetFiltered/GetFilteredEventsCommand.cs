using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Enums;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetFilteredEventsCommand : IRequest<IEnumerable<EventDto>>
    {
        public GetFilteredEventsCommand(EventFilterDto eventFilterDto) 
        {
            Filter = eventFilterDto;
        }
        public EventFilterDto Filter { get; set; }
    }
}