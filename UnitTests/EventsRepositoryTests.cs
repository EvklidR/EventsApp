using EventsService.Domain.Entities;
using EventsService.Infrastructure.MSSQL;
using EventsService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventsService.Tests.Repository
{
    public class EventRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly EventRepository _repository;

        public EventRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new EventRepository(_context);
        }

        [Fact]
        public async Task Add_Should_Add_Event_To_Database()
        {
            var newEvent = new Event { Name = "Test Event", Description = "Test Description", Location = "Test Location" };

            _repository.Add(newEvent);
            await _context.SaveChangesAsync();

            var eventInDb = await _context.Events.FindAsync(newEvent.Id);
            Assert.NotNull(eventInDb);
            Assert.Equal(newEvent.Name, eventInDb.Name);
        }

        [Fact]
        public void GetAll_Should_Return_All_Events()
        {
            var event1 = new Event { Name = "Event 1", Description = "Description 1", Location = "Location 1" };
            var event2 = new Event { Name = "Event 2", Description = "Description 2", Location = "Location 2" };
            _context.Events.AddRange(event1, event2);
            _context.SaveChanges();

            var result = _repository.GetAll().ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Name == event1.Name);
            Assert.Contains(result, e => e.Name == event2.Name);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
