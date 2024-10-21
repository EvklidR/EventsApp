using EventsApp.EventsService.Application.DTOs;
using EventsApp.EventsService.Application.Interfaces;
using EventsApp.EventsService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;


[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly string _imagePath;
    private readonly IDatabase _redisDb;

    public EventsController(IEventService eventService,
                            IConfiguration configuration,
                            IConnectionMultiplexer redis)
    {
        _imagePath = Path.Combine(configuration["ImageSettings:ImagePath"]);
        _eventService = eventService;
        _redisDb = redis.GetDatabase();
    }

    [HttpGet("get-file/{fileName}")]
    public async Task<IActionResult> GetFile(string fileName)
    {

        byte[] cachedFile = await _redisDb.StringGetAsync(fileName);
        if (cachedFile != null && cachedFile.Length > 0)
        {
            return File(cachedFile, "application/octet-stream", fileName);
        }

        var filePath = Path.Combine(_imagePath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }

        byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        await _redisDb.StringSetAsync(fileName, fileBytes, TimeSpan.FromMinutes(30));

        return File(fileBytes, "application/octet-stream", fileName);

    }

    [HttpGet("get-event-by-name/{name}")]
    public IActionResult GetEventByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Event name cannot be null or empty.");
        }


        var eventDto = _eventService.GetEventByNameAsync(name);
        if (eventDto == null)
        {
            return NotFound("Event not found.");
        }

        return Ok(eventDto);
    }

    [HttpGet("{id}")]
    public ActionResult<EventDto> GetEventById(int id)
    {

        var eventDto = _eventService.GetEventByIdAsync(id);
        if (eventDto == null)
        {
            return NotFound("Event not found.");
        }
        return Ok(eventDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<EventDto>> GetEvents([FromQuery] EventFilterDto filterDto)
    {

        var events = _eventService.GetFilteredEvents(filterDto);
        return Ok(events);

    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto, IFormFile imageFile)
    {

        var createdEvent = await _eventService.CreateEventAsync(createEventDto, imageFile);
        return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);

    }

    [Authorize(Roles = "admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateEvent(UpdateEventDto updateEventDto)
    {

        await _eventService.UpdateEventAsync(updateEventDto);
        return NoContent();

    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {

        await _eventService.DeleteEventAsync(id);
        return NoContent();

    }
}