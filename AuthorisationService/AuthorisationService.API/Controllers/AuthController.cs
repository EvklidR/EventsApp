using Microsoft.AspNetCore.Mvc;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.Models;
using AuthorisationService.Api.Filters;
using AuthorisationService.Application.DTOs;

namespace AuthorisationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserServiceFacade _userServiceFacade;

        public AuthController(IUserServiceFacade userServiceFacade)
        {
            _userServiceFacade = userServiceFacade;
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var response = await _userServiceFacade.AuthenticateAsync(loginModel);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var response = await _userServiceFacade.RegisterAsync(createUserDto);
            return Ok(response);
        }
    }
}
