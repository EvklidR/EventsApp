using Microsoft.AspNetCore.Mvc;
using AuthorisationService.Application.Models;
using Microsoft.AspNetCore.Authorization;
using AuthorisationService.Application.Interfaces.UseCases;
using System.Security.Claims;

namespace AuthorisationService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IRefreshToken _refreshToken;
        private readonly IRevokeToken _revokeToken;

        public TokenController(IRefreshToken refreshToken, IRevokeToken revokeToken)
        {
            _refreshToken = refreshToken;
            _revokeToken = revokeToken;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            var response = await _refreshToken.ExecuteAsync(tokenApiModel);
            return Ok(response);
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);

            await _revokeToken.ExecuteAsync(userId);
            return NoContent();
        }
    }
}
