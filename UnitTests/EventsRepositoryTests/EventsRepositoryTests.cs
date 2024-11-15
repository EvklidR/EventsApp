using EventsService.Domain.Entities;
using EventsService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using EventsService.Infrastructure.MSSQL;

namespace UnitTests.EventsRepositoryTests
{
    [Collection("ApplicationDbContextCollection")]
    public class EventRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly EventRepository _repository;

        public EventRepositoryTests(ApplicationDbContextFixture fixture)
        {
            _context = fixture.Context;
            _repository = new EventRepository(_context);
        }

        [Fact]
        public async Task Add_ShouldAddEventToDatabase()
        {
            // Arrange
            var newEvent = new Event
            {
                Name = "Test Event",
                Description = "Test Description",
                DateTimeHolding = DateTime.UtcNow,
                Location = "Test Location",
                MaxParticipants = 100
            };

            // Act
            _repository.Add(newEvent);
            await _context.SaveChangesAsync();

            // Assert
            var addedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Name == "Test Event");
            Assert.NotNull(addedEvent);
            Assert.Equal("Test Event", addedEvent.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnEventById()
        {
            // Arrange
            var existingEvent = new Event
            {
                Name = "Existing Event",
                Description = "Existing Description",
                DateTimeHolding = DateTime.UtcNow,
                Location = "Existing Location",
                MaxParticipants = 50
            };

            _context.Events.Add(existingEvent);
            await _context.SaveChangesAsync();

            // Act
            var fetchedEvent = await _repository.GetByIdAsync(existingEvent.Id);

            // Assert
            Assert.NotNull(fetchedEvent);
            Assert.Equal("Existing Event", fetchedEvent.Name);
        }

        [Fact]
        public async Task Delete_ShouldRemoveEventFromDatabase()
        {
            // Arrange
            var eventToDelete = new Event
            {
                Name = "Delete Event",
                Description = "Delete Description",
                DateTimeHolding = DateTime.UtcNow,
                Location = "Delete Location",
                MaxParticipants = 30
            };

            _context.Events.Add(eventToDelete);
            await _context.SaveChangesAsync();

            // Act
            _repository.Delete(eventToDelete);
            await _context.SaveChangesAsync();

            // Assert
            var deletedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventToDelete.Id);
            Assert.Null(deletedEvent);
        }
    }
}
