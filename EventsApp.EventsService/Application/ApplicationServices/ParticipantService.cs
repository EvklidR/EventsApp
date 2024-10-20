using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventsApp.EventsService.Application.DTOs;
using EventsApp.EventsService.Application.Interfaces;
using EventsApp.EventsService.Domain.Entities;
using EventsApp.EventsService.Domain.Interfaces;
using FluentValidation;

namespace EventsApp.EventsService.Application.ApplicationServices
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProfileDto> _createProfileValidator;

        public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateProfileDto> createProfileValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _createProfileValidator = createProfileValidator;
        }

        public async Task RegisterUserForEventAsync(CreateProfileDto profileDto)
        {
            var validationResult = await _createProfileValidator.ValidateAsync(profileDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var eventToRegister = _unitOfWork.Events.GetAll().FirstOrDefault(e => e.Id == profileDto.EventId);
            if (eventToRegister != null && eventToRegister.Participants.Count < eventToRegister.MaxParticipants)
            {
                var participant = _mapper.Map<ParticipantOfEvent>(profileDto);

                await _unitOfWork.Participants.AddAsync(participant);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new InvalidOperationException("Нет мест для регистрации на это событие.");
            }
        }

        public async Task UnregisterUserFromEventAsync(int eventId, int userId)
        {
            await _unitOfWork.Participants.DeleteAsync(eventId, userId);
            await _unitOfWork.CompleteAsync();
        }

        public IEnumerable<EventDto> GetUserEvents(int userId)
        {
            var userEvents = _unitOfWork.Events.GetAll()
                .Where(e => e.Participants.Any(p => p.UserId == userId))
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToList();

            return userEvents;
        }
        public async Task<ParticipantOfEventDto?> GetParticipantByIdAsync(int participantId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant == null)
            {
                return null;
            }
            return _mapper.Map<ParticipantOfEventDto>(participant);
        }

        public async Task<IEnumerable<ParticipantOfEventDto>> GetParticipantsByEventIdAsync(int eventId)
        {
            var participants = await _unitOfWork.Participants.GetByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<ParticipantOfEventDto>>(participants);
        }
    }
}