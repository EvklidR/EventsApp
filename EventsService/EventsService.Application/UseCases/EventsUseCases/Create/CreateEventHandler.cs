using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using AutoMapper;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEventHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _unitOfWork.Events.GetByNameAsync(request.EventDto.Name);

            if (existingEvent != null)
            {
                throw new AlreadyExistsException("Event with this name already exists");
            }

            var eventEntity = _mapper.Map<Event>(request.EventDto);
            eventEntity.ImageUrl = request.ImageFile;

            _unitOfWork.Events.Add(eventEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
