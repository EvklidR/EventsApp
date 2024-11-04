using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.Interfaces.UseCases;
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

        public async Task ExecuteAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null) throw new BadAuthorisationException("User not found");

            user.RefreshToken = null;
            await _userRepository.CompleteAsync();
        }

    }
}
