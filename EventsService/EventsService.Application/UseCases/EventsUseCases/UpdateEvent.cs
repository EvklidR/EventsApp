using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using FluentValidation;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class UpdateEvent : IUpdateEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public UpdateEvent(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UpdateEventDto> validator,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task ExecuteAsync(UpdateEventDto updateEventDto)
        {
            var existingEvent = _unitOfWork.Events.GetAll().FirstOrDefault(e => e.Id == updateEventDto.Id);

            if (existingEvent == null)
            {
                throw new NotFoundException("Event not found");
            }

            _mapper.Map(updateEventDto, existingEvent);
            await _unitOfWork.CompleteAsync();
        }
    }
}
