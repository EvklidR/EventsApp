using Microsoft.AspNetCore.Mvc;
using MediatR;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("check_user_by_id/{id}")]
        public async Task<IActionResult> Check(int id)
        {
            var response = await _mediator.Send(new CheckUserByIdQuery(id));
            return Ok(response);
        }
    }
}
