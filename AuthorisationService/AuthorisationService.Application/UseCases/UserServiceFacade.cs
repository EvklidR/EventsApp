using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Domain.Entities;

namespace AuthorisationService.Application.UseCases
{
    public class UserServiceFacade : IUserServiceFacade
    {
        private readonly IAddUser _addUser;
        private readonly IGetUserById _getUserById;
        private readonly IFindUserByCredentials _findUserByCredentials;
        private readonly IUpdateUser _updateUser;

        public UserServiceFacade(IAddUser addUser, IGetUserById getUserById, IFindUserByCredentials findUserByCredentials, IUpdateUser updateUser)
        {
            _addUser = addUser;
            _getUserById = getUserById;
            _findUserByCredentials = findUserByCredentials;
            _updateUser = updateUser;
        }

        public Task<User> AddUserAsync(CreateUserDto createUserDto) => _addUser.AddUserAsync(createUserDto);
        public Task<User?> GetByIdAsync(int id) => _getUserById.GetByIdAsync(id);
        public Task<User> FindUserByCredentialsAsync(string login, string password) => _findUserByCredentials.FindUserByCredentialsAsync(login, password);
        public Task<User> UpdateUserAsync(User userUp) => _updateUser.UpdateUserAsync(userUp);
    }

}
