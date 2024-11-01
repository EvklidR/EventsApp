using AuthorisationService.Application.DTOs;
using AuthorisationService.Domain.Entities;

namespace AuthorisationService.Application.Interfaces
{
    public interface IAddUser
    {
        Task<User> AddUserAsync(CreateUserDto createUserDto);
    }

}
