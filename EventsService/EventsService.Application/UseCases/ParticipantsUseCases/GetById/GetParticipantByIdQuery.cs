using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetParticipantByIdQuery : IRequest<ParticipantOfEventDto>
    {
        public int ParticipantId { get; set; }

        public GetParticipantByIdQuery(int participantId)
        {
            ParticipantId = participantId;
        }
    }
}
