using EventsApp.AuthorisationService.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventsApp.AuthorisationService.Domain.Interfaces;
using EventsApp.AuthorisationService.Application.Interfaces;
using EventsApp.AuthorisationService.Application.DTOs;
using AutoMapper;

namespace EventsApp.AuthorisationService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userService.FindUserByCredentialsAsync(loginModel.Username, loginModel.Password);
            if (user is null)
                return Unauthorized();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, loginModel.Username),
            new Claim(ClaimTypes.Role, (user.Role).ToString())
        };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);

            await _userService.UpdateUserAsync(user);

            return Ok(new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto is null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                var newUser = await _userService.AddUserAsync(createUserDto);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, createUserDto.Login),
                    new Claim(ClaimTypes.Role, newUser.Role.ToString())
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                newUser.RefreshToken = refreshToken;
                newUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);

                await _userService.UpdateUserAsync(newUser);
                return Ok(new AuthenticatedResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
