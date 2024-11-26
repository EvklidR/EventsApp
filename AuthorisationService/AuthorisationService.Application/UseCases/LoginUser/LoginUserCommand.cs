using MediatR;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.UseCases
{
    public class LoginUserCommand : IRequest<AuthenticatedResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
