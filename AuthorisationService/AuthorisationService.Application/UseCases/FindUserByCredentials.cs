using AuthorisationService.Application.Interfaces;
using AuthorisationService.Domain.Entities;
using AuthorisationService.Domain.Interfaces;

namespace AuthorisationService.Application.UseCases
{
    public class FindUserByCredentials : IFindUserByCredentials
    {
        private readonly IUserRepository _userRepository;

        public FindUserByCredentials(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> FindUserByCredentialsAsync(string login, string password)
        {
            var user = await _userRepository.FindUserByCredentialsAsync(login, password);
            if (user == null)
            {
                throw new ArgumentException("Invalid credentials");
            }

            return user;
        }
    }

}
