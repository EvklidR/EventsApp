using AuthorisationService.Application.DTOs;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.Interfaces
{
    public interface IRegisterUser
    {
        Task<AuthenticatedResponse> RegisterAsync(CreateUserDto createUserDto);
    }
}
