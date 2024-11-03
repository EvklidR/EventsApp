using EventsService.Application.DTOs;
using FluentValidation;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class CreateEvent : ICreateEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEvent(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateEventDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventDto> ExecuteAsync(CreateEventDto eventDto, string? imageFile)
        {

            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.ImageUrl = imageFile;

            _unitOfWork.Events.Add(eventEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
