using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using EventsApp.EventsService.Domain.Entities;
using EventsApp.EventsService.Infrastructure.Repositories;
using EventsApp.EventsService.Infrastructure.MSSQL;

namespace RepositoryUnitTests
{
    public class EventsRepositoryTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new EventRepository(context, "testPath");

            var newEvent = new Event
            {
                Name = "Test Event",
                Description = "Test Description",
                DateTimeHolding = DateTime.Now,
                Location = "Test Location",
                MaxParticipants = 100,
                ImageUrl = "default-image.jpg"
            };

            // Act
            await repository.AddAsync(newEvent);
            await context.SaveChangesAsync();

            // Assert
            var addedEvent = context.Events.FirstOrDefault(e => e.Name == "Test Event");
            Assert.NotNull(addedEvent);
            Assert.Equal("Test Event", addedEvent.Name);
            Assert.Equal("Test Description", addedEvent.Description);
            Assert.Equal("default-image.jpg", addedEvent.ImageUrl);
        }


        [Fact]
        public async Task GetAll_ShouldReturnAllEvents()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new EventRepository(context, "testPath");

            var event1 = new Event
            {
                Name = "Event 1",
                Description = "Description 1",
                DateTimeHolding = DateTime.Now.AddDays(1),
                Location = "Location 1",
                MaxParticipants = 50,
                ImageUrl = "image1.jpg"
            };

            var event2 = new Event
            {
                Name = "Event 2",
                Description = "Description 2",
                DateTimeHolding = DateTime.Now.AddDays(2),
                Location = "Location 2",
                MaxParticipants = 100,
                ImageUrl = "image2.jpg"
            };

            await repository.AddAsync(event1);
            await repository.AddAsync(event2);
            await context.SaveChangesAsync();

            // Act
            var events = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(events);
            Assert.Equal(2, events.Count);
            Assert.Equal("Event 1", events[0].Name);
            Assert.Equal("Event 2", events[1].Name);
            Assert.Equal("image1.jpg", events[0].ImageUrl);
            Assert.Equal("image2.jpg", events[1].ImageUrl);
        }

    }
}
