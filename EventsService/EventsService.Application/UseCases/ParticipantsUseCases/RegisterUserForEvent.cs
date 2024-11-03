using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class RegisterUserForEvent : IRegisterUserForEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterUserForEvent(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(CreateProfileDto profileDto)
        {
            var eventToRegister = await _unitOfWork.Events.GetAll()
                .FirstOrDefaultAsync(e => e.Id == profileDto.EventId);

            if (eventToRegister == null) 
            {
                throw new NotFoundException("Event not found");
            }

            var isAlreadyRegistered = await _unitOfWork.Participants
                .GetAsync(p => (p.UserId == profileDto.UserId && p.EventId == profileDto.EventId));

            if (isAlreadyRegistered.Any())
            {
                throw new BusinessLogicException("You are already registered");
            }

            if (eventToRegister.Participants.Count < eventToRegister.MaxParticipants)
            {
                var participant = _mapper.Map<ParticipantOfEvent>(profileDto);
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
