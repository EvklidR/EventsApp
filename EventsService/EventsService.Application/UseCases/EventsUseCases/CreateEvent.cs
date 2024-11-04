using EventsService.Application.DTOs;
using FluentValidation;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class CreateEvent : ICreateEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEvent(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventDto> ExecuteAsync(CreateEventDto eventDto, string? imageFile)
        {
            var existingEvents = _unitOfWork.Events.GetAll();

            var existingEvent = await existingEvents.FirstOrDefaultAsync(e => e.Name == eventDto.Name);

            if (existingEvent != null)
            {
                throw new AlreadyExistsException("Event with this name already exist");
            }

            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.ImageUrl = imageFile;

            _unitOfWork.Events.Add(eventEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
