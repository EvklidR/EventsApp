using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces.UseCases
{
    public interface IRefreshToken
    {
        Task<AuthenticatedResponse> ExecuteAsync(TokenApiModel tokenApiModel);
    }
}
