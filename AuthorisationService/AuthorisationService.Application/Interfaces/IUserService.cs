using AutoMapper;
using EventsApp.AuthorisationService.Application.DTOs;
using EventsApp.AuthorisationService.Domain.Entities;

namespace EventsApp.AuthorisationService.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);
        Task<User> AddUserAsync(CreateUserDto createUserDto);
        Task<User> FindUserByCredentialsAsync(string login, string password);
        Task<User> UpdateUserAsync(User user);

    }
}
