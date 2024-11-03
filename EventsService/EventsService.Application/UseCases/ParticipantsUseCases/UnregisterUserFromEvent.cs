using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class UnregisterUserFromEvent : IUnregisterUserFromEvent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnregisterUserFromEvent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int eventId, int userId)
        {
            var participant = await _unitOfWork.Participants
                .GetAsync(p => (p.EventId == eventId && p.UserId == userId));

            var singleParticipant = participant?.SingleOrDefault();

            if (singleParticipant == null)
            {
                throw new NotFoundException("Your registration not found");
            }

            _unitOfWork.Participants.Delete(singleParticipant);
            await _unitOfWork.CompleteAsync();
        }
    }
}
