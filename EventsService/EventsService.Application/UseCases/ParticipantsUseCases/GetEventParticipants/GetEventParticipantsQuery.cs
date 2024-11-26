using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetEventParticipantsQuery : IRequest<IEnumerable<ParticipantOfEventDto>>
    {
        public int EventId { get; set; }

        public GetEventParticipantsQuery(int eventId)
        {
            EventId = eventId;
        }
    }
}
