using MediatR;
using EventsService.Application.UseCases.ParticipantsUseCases;
using EventsService.Application.Interfaces;
using EventsService.Application.DTOs;
using EventsService.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventsService.Application.UseCases.ParticipantsUseCases.Notify
{
    public class NotifyParticipantsCommandHandler : IRequestHandler<NotifyParticipantsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public NotifyParticipantsCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task Handle(NotifyParticipantsCommand request, CancellationToken cancellationToken)
        {
            var updateEventDto = request.UpdateEventDto;

            var participants = await _unitOfWork.Participants.GetAllAsync();
            var eventParticipants = participants.Where(p => p.EventId == updateEventDto.Id).ToList();

            if (eventParticipants.Any())
            {
                foreach (var participant in eventParticipants)
                {
                    var body = $"Dear {participant.Name}, the event {updateEventDto.Name} you are participating in has been updated. " +
                               $"Current location: {updateEventDto.Location}, current date: {updateEventDto.DateTimeHolding}";
                    _emailSender.SendEmail(participant.Email, "Event Updated", body);
                }
            }
        }
    }
}
