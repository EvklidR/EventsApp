using MediatR;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.UseCases
{
    public class RefreshTokenCommand : IRequest<AuthenticatedResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public RefreshTokenCommand(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
