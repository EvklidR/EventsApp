using MediatR;
using AuthorisationService.Application.Models;
using AuthorisationService.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using AuthorisationService.Domain.Entities;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Application.UseCases
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthenticatedResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RegisterUserHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthenticatedResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var createUserDto = request.CreateUserDto;

            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                throw new AlreadyExists("A user with the same email already exists.");
            }

            existingUser = await _userRepository.GetByLoginAsync(createUserDto.Login);
            if (existingUser != null)
            {
                throw new AlreadyExists("A user with the same login already exists.");
            }

            var user = _mapper.Map<User>(createUserDto);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);

            _userRepository.AddUser(user);
            await _userRepository.CompleteAsync();

            var accessToken = _tokenService.GenerateAccessToken(user);

            return new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
