using AuthorisationService.Application.Interfaces;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Application.Models;

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

        public async Task<AuthenticatedResponse> RefreshAccessTokenAsync(TokenApiModel tokenApiModel)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
            var username = principal.Identity.Name;

            var user = await _userRepository.GetAsync(u => u.Login == username);
            if (user == null || user.RefreshToken != tokenApiModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new ArgumentException("Invalid client request");
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
