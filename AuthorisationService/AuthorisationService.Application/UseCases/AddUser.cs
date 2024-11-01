using AuthorisationService.Application.Interfaces;
using AutoMapper;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Domain.Entities;
using AuthorisationService.Domain.Interfaces;
using FluentValidation;

namespace AuthorisationService.Application.UseCases
{
    public class AddUser : IAddUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _validator;

        public AddUser(IUserRepository userRepository, IMapper mapper, IValidator<CreateUserDto> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<User> AddUserAsync(CreateUserDto createUserDto)
        {
            var validationResult = await _validator.ValidateAsync(createUserDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(createUserDto);
            _userRepository.AddUser(user);
            await _userRepository.CompleteAsync();

            return user;
        }
    }

}
