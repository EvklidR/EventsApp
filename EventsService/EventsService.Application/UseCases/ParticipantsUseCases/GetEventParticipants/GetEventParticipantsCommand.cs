using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetEventParticipantsCommand : IRequest<IEnumerable<ParticipantOfEventDto>>
    {
        public int EventId { get; set; }

        public GetEventParticipantsCommand(int eventId)
        {
            EventId = eventId;
        }
    }
}
