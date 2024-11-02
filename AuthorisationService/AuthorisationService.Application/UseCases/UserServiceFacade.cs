using AuthorisationService.Application.DTOs;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Application.UseCases
{
    public class UserServiceFacade : IUserServiceFacade
    {
        private readonly ILoginUser _loginUser;
        private readonly IRegisterUser _registerUser;
        private readonly IRefreshToken _refreshToken;
        private readonly IRevokeToken _revokeToken;

        public UserServiceFacade(ILoginUser loginUser, IRegisterUser registerUser, IRefreshToken refreshToken, IRevokeToken revokeToken)
        {
            _loginUser = loginUser;
            _registerUser = registerUser;
            _refreshToken = refreshToken;
            _revokeToken = revokeToken;
        }

        public async Task<AuthenticatedResponse> AuthenticateAsync(LoginModel loginModel)
        {
            return await _loginUser.AuthenticateAsync(loginModel);
        }

        public async Task<AuthenticatedResponse> RegisterAsync(CreateUserDto createUserDto)
        {
            return await _registerUser.RegisterAsync(createUserDto);
        }

        public async Task<AuthenticatedResponse> RefreshAccessTokenAsync(TokenApiModel tokenApiModel)
        {
            return await _refreshToken.RefreshAccessTokenAsync(tokenApiModel);
        }

        public async Task RevokeTokenAsync(string username)
        {
            await _revokeToken.RevokeAsync(username);
        }
    }
}
