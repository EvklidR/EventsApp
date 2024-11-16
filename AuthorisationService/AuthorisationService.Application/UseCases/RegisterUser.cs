using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using AuthorisationService.Application.Models;
using AuthorisationService.Domain.Entities;
using System.Security.Claims;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.Interfaces.UseCases;

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

        public async Task<AuthenticatedResponse> ExecuteAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);

            if (existingUser != null)
            {
                throw new AlreadyExistsException("A user with the same email already exists.");
            }

            existingUser = await _userRepository.GetByLoginAsync(createUserDto.Login);

            if (existingUser != null)
            {
                throw new AlreadyExistsException("A user with the same login already exists.");
            }

            var user = _mapper.Map<User>(createUserDto);

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);

            _userRepository.AddUser(user);
            await _userRepository.CompleteAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            return new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
