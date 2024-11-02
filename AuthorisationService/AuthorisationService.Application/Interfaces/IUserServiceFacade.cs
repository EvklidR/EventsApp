using AuthorisationService.Application.DTOs;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces
{
    public interface IUserServiceFacade
    {
        Task<AuthenticatedResponse> AuthenticateAsync(LoginModel loginModel);
        Task<AuthenticatedResponse> RegisterAsync(CreateUserDto createUserDto);
        Task<AuthenticatedResponse> RefreshAccessTokenAsync(TokenApiModel tokenApiModel);
        Task RevokeTokenAsync(string username);
    }
}
