using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EventsService.Application
{
    public class EventsUseCasesFacade
    {
        private readonly IGetEventById _getEventByIdHandler;
        private readonly IGetEventByName _getEventByNameHandler;
        private readonly ICreateEvent _createEventHandler;
        private readonly IUpdateEvent _updateEventHandler;
        private readonly IDeleteEvent _deleteEventHandler;
        private readonly IGetFilteredEvents _getFilteredEventsHandler;
        private readonly INotifyParticipants _notifyParticipantsHandler;
        private readonly IImageService _imageService;

        public EventsUseCasesFacade(
            IGetEventById getEventByIdHandler,
            IGetEventByName getEventByNameHandler,
            ICreateEvent createEventHandler,
            IUpdateEvent updateEventHandler,
            IDeleteEvent deleteEventHandler,
            IGetFilteredEvents getFilteredEventsHandler,
            INotifyParticipants notifyParticipantsHandler,
            IImageService imageService)
        {
            _getEventByIdHandler = getEventByIdHandler;
            _getEventByNameHandler = getEventByNameHandler;
            _createEventHandler = createEventHandler;
            _updateEventHandler = updateEventHandler;
            _deleteEventHandler = deleteEventHandler;
            _getFilteredEventsHandler = getFilteredEventsHandler;
            _notifyParticipantsHandler = notifyParticipantsHandler;
            _imageService = imageService;
        }

        public EventDto GetEventById(int id)
        {
            return _getEventByIdHandler.Execute(id);
        }

        public EventDto GetEventByName(string name)
        {
            return _getEventByNameHandler.Execute(name);
        }

        public async Task<EventDto> CreateEvent(CreateEventDto eventDto, IFormFile imageFile)
        {
            string? imageFileName = null;

            if (imageFile != null)
            {
                imageFileName = await _imageService.SaveImageAsync(imageFile);
            }

            return await _createEventHandler.ExecuteAsync(eventDto, imageFileName);
        }

        public async Task UpdateEvent(UpdateEventDto updateEventDto)
        {
            var existingEvent = _getEventByIdHandler.Execute(updateEventDto.Id);

            await _updateEventHandler.ExecuteAsync(updateEventDto);

            if (existingEvent.Location != updateEventDto.Location ||
                existingEvent.DateTimeHolding != updateEventDto.DateTimeHolding)
            {
                await _notifyParticipantsHandler.ExecuteAsync(updateEventDto);
            }
        }

        public async Task DeleteEvent(int id)
        {
            var eventEntity = _getEventByIdHandler.Execute(id);

            await _deleteEventHandler.ExecuteAsync(id);
            
            if (eventEntity.ImageUrl != null)
            {
                _imageService.DeleteImage(eventEntity.ImageUrl);
            }
        }

        public IEnumerable<EventDto>? GetFilteredEvents(EventFilterDto filterDto)
        {
            return _getFilteredEventsHandler.Execute(filterDto);
        }
    }
}
