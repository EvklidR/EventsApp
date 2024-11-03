using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.DTOs;
using AutoMapper;
using EventsService.Application.Interfaces;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class NotifyParticipants : INotifyParticipants
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public NotifyParticipants(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task ExecuteAsync(UpdateEventDto updateEventDto)
        {
            var participants = await _unitOfWork.Participants.GetAsync(p => p.EventId == updateEventDto.Id);

            if (participants != null)
            {
                foreach (var participant in participants)
                {
                    string body = $"Dear {participant.Name}, the event {updateEventDto.Name} you are participating in has been updated. " +
                                  $"Current location: {updateEventDto.Location}, current date: {updateEventDto.DateTimeHolding}";
                    _emailSender.SendEmail(participant.Email, "Event Updated", body);
                }
            }
        }
    }
}
