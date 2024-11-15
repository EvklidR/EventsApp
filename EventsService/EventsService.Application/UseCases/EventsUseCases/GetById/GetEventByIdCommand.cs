using EventsService.Application.DTOs;
using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByIdCommand : IRequest<EventDto>
    {
        public GetEventByIdCommand(int id) 
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
