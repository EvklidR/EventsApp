using MediatR;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class UnregisterUserFromEventCommandHandler : IRequestHandler<UnregisterUserFromEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnregisterUserFromEventCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UnregisterUserFromEventCommand request, CancellationToken cancellationToken)
        {
            var participants = await _unitOfWork.Participants.GetAllAsync();

            var participant = participants.FirstOrDefault(p => p.EventId == request.EventId && p.UserId == request.UserId);

            if (participant == null)
            {
                throw new NotFound("Your registration not found");
            }

            _unitOfWork.Participants.Delete(participant);
            await _unitOfWork.CompleteAsync();
        }
    }
}
