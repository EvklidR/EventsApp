using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces
{
    public interface ILoginUser
    {
        Task<AuthenticatedResponse> AuthenticateAsync(LoginModel loginModel);
    }
}
