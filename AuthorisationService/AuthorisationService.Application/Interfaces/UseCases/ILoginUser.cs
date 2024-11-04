using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces.UseCases
{
    public interface ILoginUser
    {
        Task<AuthenticatedResponse> ExecuteAsync(LoginModel loginModel);
    }
}
