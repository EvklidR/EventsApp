using AuthorisationService.Application.Interfaces;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Application.Models;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.Interfaces.UseCases;
namespace AuthorisationService.Application.UseCases
{
    public class RefreshToken : IRefreshToken
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public RefreshToken(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedResponse> ExecuteAsync(TokenApiModel tokenApiModel)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
            var username = principal.Identity?.Name;
            var user = await _userRepository.GetAsync(u => u.Login == username);

            if (user == null)
            {
                throw new BadAuthorisationException("User not found");
            }

            if (user.RefreshToken != tokenApiModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new BadAuthorisationException("RefreshToken isn't valid");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);

            return new AuthenticatedResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken
            };
        }
    }
}
