using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Diagnostics;


[Route("api/[controller]")]
[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantService _participantService;

    public ParticipantsController(IParticipantService participantService)
    {
        _participantService = participantService;
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
        await _participantService.RegisterUserForEventAsync(profile);
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
        await _participantService.UnregisterUserFromEventAsync(eventId, userId);
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
        var userEvents = _participantService.GetUserEvents(userId);
        return Ok(userEvents);

    }

    [HttpGet("participant/{participantId}")]
    public async Task<ActionResult<ParticipantOfEventDto>> GetParticipantById(int participantId)
    {

        var participant = await _participantService.GetParticipantByIdAsync(participantId);
        if (participant == null)
        {
            return NotFound();
        }
        return Ok(participant);

    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<ParticipantOfEventDto>>> GetParticipantsByEventId(int eventId)
    {

        var participants = await _participantService.GetParticipantsByEventIdAsync(eventId);
        return Ok(participants);

    }
}