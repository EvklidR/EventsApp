using Microsoft.AspNetCore.Mvc;
using AuthorisationService.Application.Models;
using AuthorisationService.Api.Filters;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Application.Interfaces.UseCases;

namespace AuthorisationService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUser _loginUser;
        private readonly IRegisterUser _registerUser;

        public AuthController(ILoginUser loginUser, IRegisterUser registerUser)
        {
            _loginUser = loginUser;
            _registerUser = registerUser;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var response = await _loginUser.ExecuteAsync(loginModel);
            return Ok(response);
        }


        [HttpPost("register")]
        [ServiceFilter(typeof(ValidateCreateUserDtoAttribute))]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var response = await _registerUser.ExecuteAsync(createUserDto);
            return Ok(response);
        }
    }
}
