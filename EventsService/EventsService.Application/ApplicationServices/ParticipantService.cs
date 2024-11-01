using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.ApplicationServices
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
            // Валидация входящих данных
            var validationResult = await _createProfileValidator.ValidateAsync(profileDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Получаем событие для регистрации
            var eventToRegister = await _unitOfWork.Events.GetAll()
                .FirstOrDefaultAsync(e => e.Id == profileDto.EventId);

            // Проверяем, зарегистрирован ли пользователь на это событие
            var isAlreadyRegistered = await _unitOfWork.Participants
                .GetByEventIdAsync(profileDto.EventId); // Получаем участников события

            if (isAlreadyRegistered.Any(p => p.UserId == profileDto.UserId)) // Проверка по UserId
            {
                throw new InvalidOperationException("Вы уже зарегистрированы на это событие.");
            }

            // Проверяем, есть ли места для регистрации
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