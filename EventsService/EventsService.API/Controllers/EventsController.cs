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
        byte[] fileBytes = await _imageService.GetImageAsync(fileName);
        return File(fileBytes, "application/octet-stream", fileName);
    }

    [HttpGet("get-event-by-name/{name}")]
    public IActionResult GetEventByName(string name)
    {
        var eventDto = _eventsFacade.GetEventByName(name);
        return Ok(eventDto);
    }

    [HttpGet("{id}")]
    public ActionResult<EventDto> GetEventById(int id)
    {
        var eventDto = _eventsFacade.GetEventById(id);
        return Ok(eventDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<EventDto>> GetEvents([FromQuery] EventFilterDto filterDto)
    {
        var events = _eventsFacade.GetFilteredEvents(filterDto);
        return Ok(events);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto, IFormFile imageFile)
    {
        var createdEvent = await _eventsFacade.CreateEvent(createEventDto, imageFile);
        return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
    }

    [Authorize(Roles = "admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateEvent(UpdateEventDto updateEventDto)
    {
        await _eventsFacade.UpdateEvent(updateEventDto);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {

        await _eventsFacade.DeleteEvent(id);
        return NoContent();

    }
}