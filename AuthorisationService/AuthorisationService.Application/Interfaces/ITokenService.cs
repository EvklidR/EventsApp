using AuthorisationService.Domain.Entities;
using System.Security.Claims;

namespace AuthorisationService.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
