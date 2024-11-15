using MediatR;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.Id);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            _unitOfWork.Events.Delete(eventEntity);
            await _unitOfWork.CompleteAsync();

        }
    }
}
