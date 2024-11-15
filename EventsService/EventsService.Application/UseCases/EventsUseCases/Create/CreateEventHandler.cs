using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using AutoMapper;
using EventsService.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using EventsService.Application.Interfaces;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateEventHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _unitOfWork.Events.GetByNameAsync(request.EventDto.Name);
            if (existingEvent != null)
            {
                throw new AlreadyExistsException("Event with this name already exists");
            }

            string? imageUrl = null;
            if (request.ImageFile != null)
            {
                imageUrl = await _imageService.SaveImageAsync(request.ImageFile);
            }

            var eventEntity = _mapper.Map<Event>(request.EventDto);
            eventEntity.ImageUrl = imageUrl;

            _unitOfWork.Events.Add(eventEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
