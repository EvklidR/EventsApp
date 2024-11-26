using MediatR;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using EventsService.Domain.Entities;
using EventsService.Application.Interfaces;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class RegisterUserForEventCommandHandler : IRequestHandler<RegisterUserForEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RegisterUserForEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task Handle(RegisterUserForEventCommand request, CancellationToken cancellationToken)
        {
            var eventToRegister = await _unitOfWork.Events.GetByIdAsync(request.ProfileDto.EventId);

            if (eventToRegister == null)
            {
                throw new NotFoundException("Event not found");
            }

            if (request.ProfileDto.UserId == null)
            {
                throw new UnauthorizedException("There is no user id");
            }

            var isUserExist = await _userService.CheckUserAsync((int)request.ProfileDto.UserId);

            if (isUserExist == false) 
            {
                throw new UnauthorizedException("User not found");
            }

            var participants = await _unitOfWork.Participants.GetAllAsync();

            var isAlreadyRegistered = participants
                .FirstOrDefault(p => p.UserId == request.ProfileDto.UserId && p.EventId == request.ProfileDto.EventId);

            if (isAlreadyRegistered != null)
            {
                throw new AlreadyExistsException("You are already registered");
            }

            if (eventToRegister.Participants.Count < eventToRegister.MaxParticipants)
            {
                var participant = _mapper.Map<ParticipantOfEvent>(request.ProfileDto);
                _unitOfWork.Participants.Add(participant);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new AlreadyExistsException("There are no places to register for this event");
            }
        }
    }
}
