using Xunit;
using Moq;
using System.Threading.Tasks;
using EventsApp.EventsService.Domain.Interfaces;
using EventsApp.EventsService.Application.ApplicationServices;
using EventsApp.EventsService.Application.DTOs;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using EventsApp.EventsService.Domain.Entities;
using System.Linq;
using EventsApp.EventsService.Application.Interfaces;

namespace ServiceUnitTests
{
    public class EventServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IParticipantService> _mockParticipantService;
        private readonly Mock<IValidator<CreateEventDto>> _mockCreateEventValidator;
        private readonly Mock<IValidator<UpdateEventDto>> _mockUpdateEventValidator;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockParticipantService = new Mock<IParticipantService>();
            _mockCreateEventValidator = new Mock<IValidator<CreateEventDto>>();
            _mockUpdateEventValidator = new Mock<IValidator<UpdateEventDto>>();

            _eventService = new EventService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockEmailSender.Object,
                _mockParticipantService.Object,
                _mockCreateEventValidator.Object,
                _mockUpdateEventValidator.Object
            );
        }

        [Fact]
        public async Task CreateEventAsync_ShouldCreateEvent_WhenValidInput()
        {
            // Arrange
            var createEventDto = new CreateEventDto { Name = "Test Event" };
            var validationResult = new ValidationResult();
            _mockCreateEventValidator.Setup(v => v.ValidateAsync(createEventDto, default))
                .ReturnsAsync(validationResult);

            var eventEntity = new Event { Id = 1, Name = "Test Event" };
            _mockMapper.Setup(m => m.Map<Event>(createEventDto)).Returns(eventEntity);
            _mockMapper.Setup(m => m.Map<EventDto>(eventEntity)).Returns(new EventDto { Id = 1, Name = "Test Event" });

            _mockUnitOfWork.Setup(u => u.Events.AddAsync(eventEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _eventService.CreateEventAsync(createEventDto, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Event", result.Name);
            _mockUnitOfWork.Verify(u => u.Events.AddAsync(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task CreateEventAsync_ShouldThrowValidationException_WhenInvalidInput()
        {
            // Arrange
            var createEventDto = new CreateEventDto { Name = "Invalid Event" };
            var validationFailure = new ValidationFailure("Name", "Name is required");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockCreateEventValidator.Setup(v => v.ValidateAsync(createEventDto, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _eventService.CreateEventAsync(createEventDto, null));
        }

        [Fact]
        public void GetEventByIdAsync_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event { Id = eventId, Name = "Test Event" };
            _mockUnitOfWork.Setup(u => u.Events.GetAll())
                .Returns(new List<Event> { eventEntity }.AsQueryable());

            _mockMapper.Setup(m => m.Map<EventDto>(eventEntity))
                .Returns(new EventDto { Id = eventId, Name = "Test Event" });

            // Act
            var result = _eventService.GetEventByIdAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
        }

        [Fact]
        public void GetEventByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            _mockUnitOfWork.Setup(u => u.Events.GetAll())
                .Returns(new List<Event>().AsQueryable());

            // Act
            var result = _eventService.GetEventByIdAsync(eventId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateEvent_WhenValidInput()
        {
            // Arrange
            var updateEventDto = new UpdateEventDto { Id = 1, Name = "Updated Event" };
            var validationResult = new ValidationResult();
            _mockUpdateEventValidator.Setup(v => v.ValidateAsync(updateEventDto, default))
                .ReturnsAsync(validationResult);

            var eventEntity = new Event { Id = 1, Name = "Old Event", DateTimeHolding = default, Location = "Old Location" };
            _mockUnitOfWork.Setup(u => u.Events.GetAll()).Returns(new List<Event> { eventEntity }.AsQueryable());

            // Act
            await _eventService.UpdateEventAsync(updateEventDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldThrowException_WhenEventNotFound()
        {
            // Arrange
            var updateEventDto = new UpdateEventDto { Id = 1, Name = "Updated Event" };

            var validationResult = new ValidationResult(); // По умолчанию IsValid будет true
            _mockUpdateEventValidator.Setup(v => v.ValidateAsync(updateEventDto, default))
                .ReturnsAsync(validationResult);

            _mockUnitOfWork.Setup(u => u.Events.GetAll()).Returns(new List<Event>().AsQueryable());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _eventService.UpdateEventAsync(updateEventDto));

            Assert.Equal("Event not found", exception.Message);
        }




        [Fact]
        public async Task DeleteEventAsync_ShouldDeleteEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event { Id = eventId, ImageUrl = "test.jpg" };
            _mockUnitOfWork.Setup(u => u.Events.GetAll())
                .Returns(new List<Event> { eventEntity }.AsQueryable());

            // Act
            await _eventService.DeleteEventAsync(eventId);

            // Assert
            _mockUnitOfWork.Verify(u => u.Events.DeleteAsync(eventId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldThrowException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            _mockUnitOfWork.Setup(u => u.Events.GetAll())
                .Returns(new List<Event>().AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _eventService.DeleteEventAsync(eventId));
        }
    }
}
