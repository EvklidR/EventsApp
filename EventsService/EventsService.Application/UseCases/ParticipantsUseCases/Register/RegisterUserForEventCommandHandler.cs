using MediatR;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using EventsService.Domain.Entities;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class RegisterUserForEventCommandHandler : IRequestHandler<RegisterUserForEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterUserForEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(RegisterUserForEventCommand request, CancellationToken cancellationToken)
        {
            var eventToRegister = await _unitOfWork.Events.GetByIdAsync(request.ProfileDto.EventId);

            if (eventToRegister == null)
            {
                throw new NotFoundException("Event not found");
            }

            var participants = await _unitOfWork.Participants.GetAllAsync();

            var isAlreadyRegistered = participants
                .FirstOrDefault(p => p.UserId == request.ProfileDto.UserId && p.EventId == request.ProfileDto.EventId);

            if (isAlreadyRegistered != null)
            {
                throw new BusinessLogicException("You are already registered");
            }

            if (eventToRegister.Participants.Count < eventToRegister.MaxParticipants)
            {
                var participant = _mapper.Map<ParticipantOfEvent>(request.ProfileDto);
                _unitOfWork.Participants.Add(participant);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new BusinessLogicException("There are no places to register for this event");
            }
        }
    }
}
