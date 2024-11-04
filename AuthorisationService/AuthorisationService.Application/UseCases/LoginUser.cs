using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.Models;
using AuthorisationService.Domain.Interfaces;
using System.Security.Claims;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.Interfaces.UseCases;

namespace AuthorisationService.Application.UseCases
{
    public class LoginUser : ILoginUser
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUser(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedResponse> ExecuteAsync(LoginModel loginModel)
        {
            var user = await _userRepository.GetAsync(u => u.Login == loginModel.Username);

            if (user == null)
            { 
                throw new BadAuthorisationException("User with such login not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginModel.Password, user.HashedPassword))
            {
                throw new BadAuthorisationException("Password and login don't match");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, loginModel.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);

            _userRepository.UpdateUser(user);
            await _userRepository.CompleteAsync();

            return new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
