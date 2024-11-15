using EventsService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventsService.Application.UseCases.ParticipantsUseCases;
using EventsService.API.Filters;

namespace EventsService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParticipantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("register")]
        [ServiceFilter(typeof(UserIdFilter))]
        [ServiceFilter(typeof(ValidateCreateProfileDtoAttribute))]
        public async Task<IActionResult> RegisterUser(CreateProfileDto profile)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            profile.UserId = userId;

            var command = new RegisterUserForEventCommand(profile);
            await _mediator.Send(command);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("unregister/{eventId}")]
        [ServiceFilter(typeof(UserIdFilter))]
        public async Task<IActionResult> UnregisterUser(int eventId)
        {
            var userId = (int)HttpContext.Items["UserId"]!;

            var command = new UnregisterUserFromEventCommand(eventId, userId);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("get-participant-by-id/{participantId}")]
        public async Task<ActionResult<ParticipantOfEventDto>> GetParticipantById(int participantId)
        {
            var query = new GetParticipantByIdCommand(participantId);
            var participant = await _mediator.Send(query);

            return Ok(participant);
        }

        [HttpGet("get-event-participants/{eventId}")]
        public async Task<ActionResult<IEnumerable<ParticipantOfEventDto>>> GetParticipantsByEventId(int eventId)
        {
            var query = new GetEventParticipantsCommand(eventId);
            var participants = await _mediator.Send(query);

            return Ok(participants);
        }
    }
}
