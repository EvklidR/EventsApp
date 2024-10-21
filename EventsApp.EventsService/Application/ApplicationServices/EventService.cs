using EventsApp.EventsService.Domain.Entities;
using EventsApp.EventsService.Domain.Interfaces;
using EventsApp.EventsService.Application.Interfaces;
using EventsApp.EventsService.Application.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;

namespace EventsApp.EventsService.Application.ApplicationServices
{
    public class EventService : IEventService
    {
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IParticipantService _participants;
        private readonly IValidator<CreateEventDto> _createEventValidator;
        private readonly IValidator<UpdateEventDto> _updateEventValidator;

        public EventService(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IEmailSender emailSender,
                            IParticipantService participants,
                            IValidator<CreateEventDto> createEventValidator,
                            IValidator<UpdateEventDto> updateEventValidator)
        {
            _participants = participants;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailSender = emailSender;
            _createEventValidator = createEventValidator;
            _updateEventValidator = updateEventValidator;
        }

        public async Task<EventDto> CreateEventAsync(CreateEventDto eventDto, IFormFile imageFile)
        {
            var validationResult = await _createEventValidator.ValidateAsync(eventDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var eventEntity = _mapper.Map<Event>(eventDto);

            if (imageFile != null)
            {
                var imageFileName = await _unitOfWork.Events.SaveImageAsync(imageFile);
                eventEntity.ImageUrl = imageFileName;
            }

            await _unitOfWork.Events.AddAsync(eventEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }

        public EventDto GetEventByIdAsync(int id)
        {
            var eventEntity = _unitOfWork.Events.GetAll().FirstOrDefault(e => e.Id == id);

            if (eventEntity == null)
            {
                return null;
            }
            return _mapper.Map<EventDto>(eventEntity);
        }

        public EventDto GetEventByNameAsync(string name)
        {
            var eventEntity = _unitOfWork.Events.GetAll().FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            return _mapper.Map<EventDto>(eventEntity);
        }


        public async Task UpdateEventAsync(UpdateEventDto updateEventDto)
        {
            var validationResult = await _updateEventValidator.ValidateAsync(updateEventDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingEvent = _unitOfWork.Events.GetAll().FirstOrDefault(e => e.Id == updateEventDto.Id);

            if (existingEvent == null)
                throw new Exception("Event not found");

            var oldDateTime = existingEvent.DateTimeHolding;
            var oldLocation = existingEvent.Location;

            _mapper.Map(updateEventDto, existingEvent);
            await _unitOfWork.CompleteAsync();

            if (oldDateTime != existingEvent.DateTimeHolding || oldLocation != existingEvent.Location)
            {
                var participants = await _participants.GetParticipantsByEventIdAsync(existingEvent.Id);
                foreach (var participant in participants)
                {
                    string body = $"Уважаемый {participant.Name}! Данные о мероприятии {existingEvent.Name}," +
                                  $" в котором вы участвуете, изменились. " +
                                  $"Сообщаем, что теперь место проведения: {existingEvent.Location}," +
                                  $" время проведения: {existingEvent.DateTimeHolding}";
                    _emailSender.SendEmail(participant.Email, "Event Updated", body);
                }
            }
        }



        public async Task DeleteEventAsync(int id)
        {
            var eventEntity = _unitOfWork.Events.GetAll().FirstOrDefault(e => e.Id == id);
            if (eventEntity == null)
            {
                throw new Exception("Event not found");
            }

            if (!string.IsNullOrEmpty(eventEntity.ImageUrl))
            {
                _unitOfWork.Events.DeleteImage(eventEntity.ImageUrl);
            }

            await _unitOfWork.Events.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public IEnumerable<EventDto> GetFilteredEvents(EventFilterDto filterDto)
        {
            var query = _unitOfWork.Events.GetAll();

            if (filterDto.Date.HasValue)
            {
                query = query.Where(e => e.DateTimeHolding.Date == filterDto.Date.Value.Date);
            }

            if (!string.IsNullOrEmpty(filterDto.Location))
            {
                query = query.Where(e => e.Location.ToLower().Contains(filterDto.Location.ToLower()));
            }

            if (filterDto.Category.HasValue)
            {
                query = query.Where(e => e.Category == filterDto.Category.Value);
            }

            return query
                .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}