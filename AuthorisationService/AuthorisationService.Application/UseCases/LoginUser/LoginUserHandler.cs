using MediatR;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Application.Models;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Application.UseCases
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthenticatedResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByLoginAsync(request.Username);

            if (user == null)
            {
                throw new Unauthorized("User with such login not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                throw new Unauthorized("Password and login don't match");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
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
