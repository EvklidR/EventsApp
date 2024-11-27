using Moq;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Application.Interfaces;
using EventsService.Application.UseCases.EventsUseCases;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using AutoMapper;

namespace EventsService.UseCasesTests
{
    public class EventHandlersTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<IMapper> _mockMapper;

        public EventHandlersTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockImageService = new Mock<IImageService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateEventHandler_ShouldCreateEvent_WhenDataIsValid()
        {
            // Arrange
            var createEventDto = new CreateEventDto
            {
                Name = "Test Event",
                Description = "Test Description",
                Location = "Test Location"
            };

            var eventEntity = new Event { Name = createEventDto.Name };
            var eventDto = new EventDto { Name = createEventDto.Name };

            _mockUnitOfWork.Setup(u => u.Events.GetByNameAsync(createEventDto.Name))
                .ReturnsAsync((Event)null);
            _mockMapper.Setup(m => m.Map<Event>(It.IsAny<CreateEventDto>()))
                .Returns(eventEntity);
            _mockMapper.Setup(m => m.Map<EventDto>(It.IsAny<Event>()))
                .Returns(eventDto);

            var handler = new CreateEventHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockImageService.Object);

            // Act
            var result = await handler.Handle(new CreateEventCommand(createEventDto, null), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Event", result.Name);
            _mockUnitOfWork.Verify(u => u.Events.Add(It.IsAny<Event>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateEventHandler_ShouldThrowException_WhenEventWithNameExists()
        {
            // Arrange
            var createEventDto = new CreateEventDto { Name = "Duplicate Event" };
            var existingEvent = new Event { Name = createEventDto.Name };

            _mockUnitOfWork.Setup(u => u.Events.GetByNameAsync(createEventDto.Name))
                .ReturnsAsync(existingEvent);

            var handler = new CreateEventHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockImageService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyExists>(() =>
                handler.Handle(new CreateEventCommand(createEventDto, null), CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.Events.Add(It.IsAny<Event>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteEventHandler_ShouldDeleteEvent_WhenEventExists()
        {
            // Arrange
            var eventEntity = new Event { Id = 1, ImageUrl = "test.jpg" };

            _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventEntity.Id))
                .ReturnsAsync(eventEntity);

            var handler = new DeleteEventHandler(_mockUnitOfWork.Object, _mockImageService.Object);

            // Act
            await handler.Handle(new DeleteEventCommand(eventEntity.Id), CancellationToken.None);

            // Assert
            _mockUnitOfWork.Verify(u => u.Events.Delete(eventEntity), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
            _mockImageService.Verify(i => i.DeleteImage(eventEntity.ImageUrl), Times.Once);
        }

        [Fact]
        public async Task DeleteEventHandler_ShouldThrowException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventId))
                .ReturnsAsync((Event)null);

            var handler = new DeleteEventHandler(_mockUnitOfWork.Object, _mockImageService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFound>(() =>
                handler.Handle(new DeleteEventCommand(eventId), CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.Events.Delete(It.IsAny<Event>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetEventByIdHandler_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventEntity = new Event { Id = 1, Name = "Test Event" };
            var eventDto = new EventDto { Name = "Test Event" };

            _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventEntity.Id))
                .ReturnsAsync(eventEntity);
            _mockMapper.Setup(m => m.Map<EventDto>(eventEntity))
                .Returns(eventDto);

            var handler = new GetEventByIdHandler(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetEventByIdQuery(eventEntity.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Event", result.Name);
        }

        [Fact]
        public async Task GetEventByIdHandler_ShouldThrowException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _mockUnitOfWork.Setup(u => u.Events.GetByIdAsync(eventId))
                .ReturnsAsync((Event)null);

            var handler = new GetEventByIdHandler(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFound>(() =>
                handler.Handle(new GetEventByIdQuery(eventId), CancellationToken.None));
        }
    }
}
