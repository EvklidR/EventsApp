using AuthorisationService.Application.DTOs;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces.UseCases
{
    public interface IRegisterUser
    {
        Task<AuthenticatedResponse> ExecuteAsync(CreateUserDto createUserDto);
    }
}
