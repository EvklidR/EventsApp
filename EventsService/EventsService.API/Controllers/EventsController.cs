using EventsService.API.Filters;
using EventsService.Application.DTOs;
using EventsService.Application.UseCases.EventsUseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventsService.Application.Interfaces;

namespace EventsService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IImageService _imageService;

        public EventsController(IMediator mediator, IImageService imageService)
        {
            _mediator = mediator;
            _imageService = imageService;
        }

        [HttpGet("get-file/{fileName}")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            byte[] fileBytes = await _imageService.GetCashedImageAsync(fileName);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        [HttpGet("get-event-by-name/{name}")]
        public async Task<IActionResult> GetEventByName(GetEventByNameQuery query)
        {
            var eventDto = await _mediator.Send(query);
            return Ok(eventDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(GetEventByIdQuery query)
        {
            var eventDto = await _mediator.Send(query);
            return Ok(eventDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents(GetFilteredEventsQuery query)
        {
            var events = await _mediator.Send(query);
            return Ok(events);
        }

        [Authorize]
        [HttpGet("user-events")]
        [ServiceFilter(typeof(UserIdFilter))]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetUserEvents()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var query = new GetUserEventsQuery(userId);
            var userEvents = await _mediator.Send(query);
            return Ok(userEvents);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventCommand command)
        {
            var createdEvent = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateEvent(UpdateEventCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(DeleteEventCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
