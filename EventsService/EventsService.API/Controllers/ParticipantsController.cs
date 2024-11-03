using EventsService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventsService.Application;


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
    public async Task<IActionResult> RegisterUser(CreateProfileDto profile)
    {

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId = int.Parse(userIdClaim.Value);
        profile.UserId = userId;
        await _participantsFacade.RegisterUserForEventAsync(profile);
        return NoContent();

    }

    [Authorize]
    [HttpDelete("{eventId}")]
    public async Task<IActionResult> UnregisterUser(int eventId)
    {

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId = int.Parse(userIdClaim.Value);
        await _participantsFacade.UnregisterUserFromEventAsync(eventId, userId);
        return NoContent();

    }

    [Authorize]
    [HttpGet("user")]
    public ActionResult<IEnumerable<EventDto>> GetUserEvents()
    {

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId = int.Parse(userIdClaim.Value);
        var userEvents = _participantsFacade.GetUserEvents(userId);
        return Ok(userEvents);

    }

    [HttpGet("participant/{participantId}")]
    public async Task<ActionResult<ParticipantOfEventDto>> GetParticipantById(int participantId)
    {

        var participant = await _participantsFacade.GetParticipantByIdAsync(participantId);
        if (participant == null)
        {
            return NotFound();
        }
        return Ok(participant);

    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<ParticipantOfEventDto>>> GetParticipantsByEventId(int eventId)
    {

        var participants = await _participantsFacade.GetEventParticipantsAsync(eventId);
        return Ok(participants);

    }
}