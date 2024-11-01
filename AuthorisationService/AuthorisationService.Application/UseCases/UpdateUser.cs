using AuthorisationService.Application.Interfaces;
using AuthorisationService.Domain.Entities;
using AuthorisationService.Domain.Interfaces;

namespace AuthorisationService.Application.UseCases
{
    public class UpdateUser : IUpdateUser
    {
        private readonly IUserRepository _userRepository;

        public UpdateUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> UpdateUserAsync(User userUp)
        {
            var user = await _userRepository.GetAsync(u => u.Id == userUp.Id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            _userRepository.UpdateUser(user);
            await _userRepository.CompleteAsync();

            return user;
        }
    }

}
