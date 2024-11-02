using AuthorisationService.Application.Interfaces;
using AuthorisationService.Domain.Interfaces;

namespace AuthorisationService.Application.UseCases
{
    public class RevokeToken : IRevokeToken
    {
        private readonly IUserRepository _userRepository;

        public RevokeToken(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RevokeAsync(string username)
        {
            var user = await _userRepository.GetAsync(u => u.Login == username);
            if (user == null) throw new ArgumentException("User not found");

            user.RefreshToken = null;
            await _userRepository.CompleteAsync();
        }

    }
}
