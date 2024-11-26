using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MediatR;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);

            await _mediator.Send(new RevokeTokenCommand(userId));
            return NoContent();
        }
    }
}
