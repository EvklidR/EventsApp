//using EventsService.Application.DTOs;
//using EventsService.Application.Interfaces;
//using EventsService.Application.Interfaces.EventsUseCases;
//using EventsService.Application.Interfaces.ParticipantsUseCases;
//using EventsService.Domain.Entities;
//using EventsService.Domain.Interfaces;
//using Microsoft.AspNetCore.Http;

//namespace EventsService.Application
//{
//    public class EventsUseCasesFacade
//    {
//        private readonly IGetEventById _getEventByIdHandler;
//        private readonly IGetEventByName _getEventByNameHandler;
//        private readonly ICreateEvent _createEventHandler;
//        private readonly IUpdateEvent _updateEventHandler;
//        private readonly IDeleteEvent _deleteEventHandler;
//        private readonly IGetFilteredEvents _getFilteredEventsHandler;
//        private readonly INotifyParticipants _notifyParticipantsHandler;
//        private readonly IImageService _imageService;
//        private readonly IGetUserEvents _getUserEventsHandler;

//        public EventsUseCasesFacade(
//            IGetEventById getEventByIdHandler,
//            IGetEventByName getEventByNameHandler,
//            ICreateEvent createEventHandler,
//            IUpdateEvent updateEventHandler,
//            IDeleteEvent deleteEventHandler,
//            IGetFilteredEvents getFilteredEventsHandler,
//            INotifyParticipants notifyParticipantsHandler,
//            IImageService imageService,
//            IGetUserEvents getUserEventsHandler)
//        {
//            _getEventByIdHandler = getEventByIdHandler;
//            _getEventByNameHandler = getEventByNameHandler;
//            _createEventHandler = createEventHandler;
//            _updateEventHandler = updateEventHandler;
//            _deleteEventHandler = deleteEventHandler;
//            _getFilteredEventsHandler = getFilteredEventsHandler;
//            _notifyParticipantsHandler = notifyParticipantsHandler;
//            _imageService = imageService;
//            _getUserEventsHandler = getUserEventsHandler;
//        }

//        public async Task<EventDto> GetEventByIdAsync(int id)
//        {
//            return await _getEventByIdHandler.ExecuteAsync(id);
//        }

//        public async Task<EventDto> GetEventByNameAsync(string name)
//        {
//            return await _getEventByNameHandler.ExecuteAsync(name);
//        }

//        public async Task<IEnumerable<EventDto>?> GetUserEventsAsync(int userId)
//        {
//            return await _getUserEventsHandler.ExecuteAsync(userId);
//        }

//        public async Task<EventDto> CreateEventAsync(CreateEventDto eventDto, IFormFile? imageFile)
//        {
//            string? imageFileName = null;

//            if (imageFile != null)
//            {
//                imageFileName = await _imageService.SaveImageAsync(imageFile);
//            }

//            return await _createEventHandler.ExecuteAsync(eventDto, imageFileName);
//        }

//        public async Task UpdateEventAsync(UpdateEventDto updateEventDto)
//        {
//            var existingEvent = await _getEventByIdHandler.ExecuteAsync(updateEventDto.Id);

//            await _updateEventHandler.ExecuteAsync(updateEventDto);

//            if (existingEvent.Location != updateEventDto.Location ||
//                existingEvent.DateTimeHolding != updateEventDto.DateTimeHolding)
//            {
//                await _notifyParticipantsHandler.ExecuteAsync(updateEventDto);
//            }
//        }

//        public async Task DeleteEventAsync(int id)
//        {
//            var eventEntity = await _getEventByIdHandler.ExecuteAsync(id);

//            await _deleteEventHandler.ExecuteAsync(id);
            
//            if (eventEntity.ImageUrl != null)
//            {
//                _imageService.DeleteImage(eventEntity.ImageUrl);
//            }
//        }

//        public async Task<IEnumerable<EventDto>?> GetFilteredEventsAsync(EventFilterDto filterDto)
//        {
//            return await _getFilteredEventsHandler.ExecuteAsync(filterDto);
//        }
//    }
//}
