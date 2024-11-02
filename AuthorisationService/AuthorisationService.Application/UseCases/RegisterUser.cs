using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using AuthorisationService.Application.Models;
using AuthorisationService.Domain.Entities;
using System.Security.Claims;

namespace AuthorisationService.Application.UseCases
{
    public class RegisterUser : IRegisterUser
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _validator;

        public RegisterUser(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, IValidator<CreateUserDto> validator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<AuthenticatedResponse> RegisterAsync(CreateUserDto createUserDto)
        {
            var validationResult = await _validator.ValidateAsync(createUserDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(createUserDto);
            user.RefreshToken = null; // Initialize refresh token

            _userRepository.AddUser(user);
            await _userRepository.CompleteAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
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
