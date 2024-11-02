using Microsoft.AspNetCore.Mvc;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.Models;
using Microsoft.AspNetCore.Authorization;
using AuthorisationService.Api.Filters;

namespace AuthorisationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserServiceFacade _userServiceFacade;

        public TokenController(IUserServiceFacade userServiceFacade)
        {
            _userServiceFacade = userServiceFacade;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            var response = await _userServiceFacade.RefreshAccessTokenAsync(tokenApiModel);
            return Ok(response);
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            await _userServiceFacade.RevokeTokenAsync(username);
            return NoContent();
        }
    }
}
