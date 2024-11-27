using MediatR;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;
using EventsService.Application.Interfaces;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteEventHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.Id);

            if (eventEntity == null)
            {
                throw new NotFound("Event not found");
            }

            if (!string.IsNullOrEmpty(eventEntity.ImageUrl))
            {
                _imageService.DeleteImage(eventEntity.ImageUrl);
            }

            _unitOfWork.Events.Delete(eventEntity);
            await _unitOfWork.CompleteAsync();
        }
    }
}
