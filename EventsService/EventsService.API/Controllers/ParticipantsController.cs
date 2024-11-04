using EventsService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventsService.Application;
using EventsService.API.Filters;

[Route("api/[controller]")]
[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly ParticipantsUseCasesFacade _participantsFacade;

    public ParticipantsController(ParticipantsUseCasesFacade participantsFacade)
    {
        _participantsFacade = participantsFacade;
    }

    [Authorize]
    [HttpPost]
    [ServiceFilter(typeof(UserIdFilter))]
    [ServiceFilter(typeof(ValidateCreateProfileDtoAttribute))]
    public async Task<IActionResult> RegisterUser(CreateProfileDto profile)
    {
        var userId = (int)HttpContext.Items["UserId"]!;
        profile.UserId = userId;

        await _participantsFacade.RegisterUserForEventAsync(profile);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{eventId}")]
    [ServiceFilter(typeof(UserIdFilter))]
    public async Task<IActionResult> UnregisterUser(int eventId)
    {
        var userId = (int)HttpContext.Items["UserId"]!;

        await _participantsFacade.UnregisterUserFromEventAsync(eventId, userId);
        return NoContent();
    }

    [HttpGet("participant/{participantId}")]
    public async Task<ActionResult<ParticipantOfEventDto>> GetParticipantById(int participantId)
    {
        var participant = await _participantsFacade.GetParticipantByIdAsync(participantId);
        return Ok(participant);
    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<ParticipantOfEventDto>>> GetParticipantsByEventId(int eventId)
    {
        var participants = await _participantsFacade.GetEventParticipantsAsync(eventId);
        return Ok(participants);
    }
}
