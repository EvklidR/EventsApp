using Moq;
using Microsoft.AspNetCore.Mvc;
using EventsService.Application.Interfaces;
using EventsService.Application.DTOs;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace ControllerUnitTests
{
    public class EventsControllerTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly Mock<IConnectionMultiplexer> _mockRedis;
        private readonly Mock<IDatabase> _mockRedisDb;
        private readonly IConfiguration _mockConfig;
        private readonly EventsController _controller;

        public EventsControllerTests()
        {
            _mockEventService = new Mock<IEventService>();
            _mockRedis = new Mock<IConnectionMultiplexer>();
            _mockRedisDb = new Mock<IDatabase>();
            _mockRedis.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockRedisDb.Object);

            var inMemorySettings = new Dictionary<string, string> {
            {"ImageSettings:ImagePath", "test/path"}
        };
            _mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new EventsController(_mockEventService.Object, _mockConfig, _mockRedis.Object);
        }


        [Fact]
        public void GetEventById_ReturnsOkResult_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDto { Id = eventId, Name = "Test Event" };
            _mockEventService.Setup(s => s.GetEventByIdAsync(eventId)).Returns(eventDto);

            // Act
            var result = _controller.GetEventById(eventId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var returnValue = okResult.Value as EventDto;
            Assert.NotNull(returnValue);
            Assert.Equal(eventId, returnValue.Id);
        }

        [Fact]
        public void GetEventById_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            _mockEventService.Setup(s => s.GetEventByIdAsync(eventId)).Returns((EventDto)null);

            // Act
            var result = _controller.GetEventById(eventId);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }


        [Fact]
        public void GetEventByName_ReturnsOkResult_WhenEventExists()
        {
            // Arrange
            var eventName = "Test Event";
            var eventDto = new EventDto { Name = eventName };
            _mockEventService.Setup(s => s.GetEventByNameAsync(eventName)).Returns(eventDto);

            // Act
            var result = _controller.GetEventByName(eventName);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var returnValue = okResult.Value as EventDto;
            Assert.NotNull(returnValue);
            Assert.Equal(eventName, returnValue.Name);
        }

        [Fact]
        public void GetEventByName_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventName = "NonExistent Event";
            _mockEventService.Setup(s => s.GetEventByNameAsync(eventName)).Returns((EventDto)null);

            // Act
            var result = _controller.GetEventByName(eventName);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void GetEventByName_ReturnsBadRequest_WhenEventNameIsEmpty()
        {
            // Act
            var result = _controller.GetEventByName("");

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }


        [Fact]
        public async Task CreateEvent_ReturnsCreatedAtAction_WhenEventCreated()
        {
            // Arrange
            var createEventDto = new CreateEventDto { Name = "New Event" };
            var createdEventDto = new EventDto { Id = 1, Name = "New Event" };
            _mockEventService.Setup(s => s.CreateEventAsync(createEventDto, null)).ReturnsAsync(createdEventDto);

            // Act
            var result = await _controller.CreateEvent(createEventDto, null);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(nameof(_controller.GetEventById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);

            var returnValue = createdAtActionResult.Value as EventDto;
            Assert.NotNull(returnValue);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("New Event", returnValue.Name);
        }


        [Fact]
        public async Task UpdateEvent_ReturnsNoContent_WhenEventUpdated()
        {
            // Arrange
            var updateEventDto = new UpdateEventDto { Id = 1, Name = "Updated Event" };

            // Act
            var result = await _controller.UpdateEvent(updateEventDto);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }


        [Fact]
        public async Task DeleteEvent_ReturnsNoContent_WhenEventDeleted()
        {
            // Arrange
            var eventId = 1;

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}