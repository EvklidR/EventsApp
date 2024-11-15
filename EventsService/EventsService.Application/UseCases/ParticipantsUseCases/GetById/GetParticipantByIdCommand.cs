using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetParticipantByIdCommand : IRequest<ParticipantOfEventDto>
    {
        public int ParticipantId { get; set; }

        public GetParticipantByIdCommand(int participantId)
        {
            ParticipantId = participantId;
        }
    }
}
