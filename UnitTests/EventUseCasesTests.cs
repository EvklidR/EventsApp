using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using EventsService.Application.DTOs;
using EventsService.Application.UseCases.EventsUseCases;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;
using System.Threading.Tasks;
using EventsService.Infrastructure.Repositories;
using EventsService.Infrastructure.MSSQL;

namespace EventsService.Tests.UseCases;

public class EventUseCasesTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EventUseCasesTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateEventDto, Event>();
            cfg.CreateMap<Event, EventDto>();
            cfg.CreateMap<UpdateEventDto, Event>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateEvent_ShouldReturnEventDto_WhenEventIsCreated()
    {
        var eventDto = new CreateEventDto
        {
            Name = "New Event",
            Description = "New Description",
            Location = "New Location"
        };

        var unitOfWork = new UnitOfWork(_context);
        var createEvent = new CreateEvent(unitOfWork, _mapper);

        var result = await createEvent.ExecuteAsync(eventDto, null);

        Assert.NotNull(result);
        Assert.Equal("New Event", result.Name);
    }

    [Fact]
    public async Task GetEventById_ShouldReturnEventDto_WhenEventExists()
    {
        var eventEntity = new Event
        {
            Id = 1,
            Name = "Existing Event",
            Description = "Some description",
            Location = "Some location"
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();

        var unitOfWork = new UnitOfWork(_context);
        var getEventById = new GetEventById(unitOfWork, _mapper);

        var result = await getEventById.ExecuteAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Existing Event", result.Name);
    }

    [Fact]
    public async Task UpdateEvent_ShouldUpdateEvent_WhenEventExists()
    {
        var eventEntity = new Event
        {
            Id = 1,
            Name = "Event to Update",
            Description = "Old description",
            Location = "Old location"
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();

        var updateEventDto = new UpdateEventDto
        {
            Id = 1,
            Name = "Updated Event",
            Description = "Updated description",
            Location = "Updated location"
        };

        var unitOfWork = new UnitOfWork(_context);
        var updateEvent = new UpdateEvent(unitOfWork, _mapper);

        await updateEvent.ExecuteAsync(updateEventDto);

        var updatedEvent = await _context.Events.FindAsync(1);
        Assert.NotNull(updatedEvent);
        Assert.Equal("Updated Event", updatedEvent.Name);
    }

    [Fact]
    public async Task DeleteEvent_ShouldDeleteEvent_WhenEventExists()
    {
        var eventEntity = new Event
        {
            Id = 1,
            Name = "Event to Delete",
            Description = "Description",
            Location = "Location"
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();

        var unitOfWork = new UnitOfWork(_context);
        var deleteEvent = new DeleteEvent(unitOfWork);

        await deleteEvent.ExecuteAsync(1);

        var eventInDb = await _context.Events.FindAsync(1);
        Assert.Null(eventInDb);
    }

    [Fact]
    public async Task DeleteEvent_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 1;
        var unitOfWork = new UnitOfWork(_context);
        var deleteEvent = new DeleteEvent(unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => deleteEvent.ExecuteAsync(eventId));
    }

    [Fact]
    public async Task UpdateEvent_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var updateEventDto = new UpdateEventDto { Id = 1, Name = "Updated Event" };

        var unitOfWork = new UnitOfWork(_context);
        var updateEvent = new UpdateEvent(unitOfWork, _mapper);

        await Assert.ThrowsAsync<NotFoundException>(() => updateEvent.ExecuteAsync(updateEventDto));
    }

    [Fact]
    public async Task GetEventById_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 1;

        var unitOfWork = new UnitOfWork(_context);
        var getEventById = new GetEventById(unitOfWork, _mapper);

        await Assert.ThrowsAsync<NotFoundException>(() => getEventById.ExecuteAsync(eventId));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
