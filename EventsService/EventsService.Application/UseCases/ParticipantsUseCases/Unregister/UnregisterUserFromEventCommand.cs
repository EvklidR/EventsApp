using MediatR;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class UnregisterUserFromEventCommand : IRequest
    {
        public int EventId { get; set; }
        public int UserId { get; set; }

        public UnregisterUserFromEventCommand(int eventId, int userId)
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}
