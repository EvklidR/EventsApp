using EventsService.API.Filters;
using EventsService.Application;
using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly EventsUseCasesFacade _eventsFacade;
    private readonly IImageService _imageService;

    public EventsController(EventsUseCasesFacade eventsUseCasesFacade, IImageService imageService)
    {
        _eventsFacade = eventsUseCasesFacade;
        _imageService = imageService;
    }

    [HttpGet("get-file/{fileName}")]
    public async Task<IActionResult> GetFile(string fileName)
    {
        byte[] fileBytes = await _imageService.GetCashedImageAsync(fileName);
        return File(fileBytes, "application/octet-stream", fileName);
    }

    [HttpGet("get-event-by-name/{name}")]
    public async Task<IActionResult> GetEventByName(string name)
    {
        var eventDto = await _eventsFacade.GetEventByNameAsync(name);
        return Ok(eventDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEventById(int id)
    {
        var eventDto = await _eventsFacade.GetEventByIdAsync(id);
        return Ok(eventDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] EventFilterDto filterDto)
    {
        var events = await _eventsFacade.GetFilteredEventsAsync(filterDto);
        return Ok(events);
    }

    [Authorize]
    [HttpGet("user")]
    [ServiceFilter(typeof(UserIdFilter))]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetUserEvents()
    {
        var userId = (int)HttpContext.Items["UserId"]!;

        var userEvents = await _eventsFacade.GetUserEventsAsync(userId);
        return Ok(userEvents);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ServiceFilter(typeof(ValidateCreateEventDtoAttribute))]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto, IFormFile? imageFile)
    {
        var createdEvent = await _eventsFacade.CreateEventAsync(createEventDto, imageFile);
        return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
    }

    [Authorize(Roles = "admin")]
    [HttpPut]
    [ServiceFilter(typeof(ValidateUpdateEventDtoAttribute))]
    public async Task<IActionResult> UpdateEvent(UpdateEventDto updateEventDto)
    {
        await _eventsFacade.UpdateEventAsync(updateEventDto);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {

        await _eventsFacade.DeleteEventAsync(id);
        return NoContent();

    }
}