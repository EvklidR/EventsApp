using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using FluentValidation;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class UpdateEvent : IUpdateEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEvent(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(UpdateEventDto updateEventDto)
        {
            var existingEvent = await _unitOfWork.Events.GetAll().FirstOrDefaultAsync(e => e.Id == updateEventDto.Id);

            if (existingEvent == null)
            {
                throw new NotFoundException("Event not found");
            }

            _mapper.Map(updateEventDto, existingEvent);
            await _unitOfWork.CompleteAsync();
        }
    }
}
