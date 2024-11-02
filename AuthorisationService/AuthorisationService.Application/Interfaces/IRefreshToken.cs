using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces
{
    public interface IRefreshToken
    {
        Task<AuthenticatedResponse> RefreshAccessTokenAsync(TokenApiModel tokenApiModel);
    }
}
