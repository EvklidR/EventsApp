using MediatR;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.Models;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Application.UseCases
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticatedResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity?.Name;

            if (username == null)
            {
                throw new Unauthorized("AccessToken isn't valid");
            }

            var user = await _userRepository.GetByLoginAsync(username!);
            if (user == null)
            {
                throw new Unauthorized("User not found");
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Unauthorized("RefreshToken isn't valid");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            return new AuthenticatedResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken
            };
        }
    }
}
