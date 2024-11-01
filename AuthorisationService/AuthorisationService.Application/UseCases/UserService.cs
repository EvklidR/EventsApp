//using FluentValidation;
//using EventsApp.AuthorisationService.Application.DTOs;
//using EventsApp.AuthorisationService.Domain.Entities;
//using EventsApp.AuthorisationService.Domain.Interfaces;
//using EventsApp.AuthorisationService.Application.Interfaces;
//using AutoMapper;

//namespace EventsApp.AuthorisationService.Application.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IMapper _mapper;
//        private readonly IValidator<CreateUserDto> _validator;

//        public UserService(IUserRepository userRepository, IMapper mapper, IValidator<CreateUserDto> validator)
//        {
//            _userRepository = userRepository;
//            _mapper = mapper;
//            _validator = validator;
//        }

//        public async Task<User?> GetByIdAsync(int id)
//        {
//            return await _userRepository.GetAsync(u => u.Id == id);
//        }

//        public async Task<User> AddUserAsync(CreateUserDto createUserDto)
//        {
//            var validationResult = await _validator.ValidateAsync(createUserDto);
//            if (!validationResult.IsValid)
//            {
//                throw new ValidationException(validationResult.Errors);
//            }

//            var user = _mapper.Map<User>(createUserDto);

//            _userRepository.AddUser(user);
//            await _userRepository.CompleteAsync();

//            return user;
//        }

//        public async Task<User> FindUserByCredentialsAsync(string login, string password)
//        {
//            var user = await _userRepository.FindUserByCredentialsAsync(login, password);
//            if (user == null)
//            {
//                throw new ArgumentException("Invalid credentials");
//            }

//            return user;
//        }

//        public async Task<User> UpdateUserAsync(User userUp)
//        {
//            var user = await _userRepository.GetAsync(u => u.Id == userUp.Id);

//            if (user == null)
//            {
//                throw new ArgumentException("User not found");
//            }

//            _userRepository.UpdateUser(user);
//            await _userRepository.CompleteAsync();

//            return user;
//        }
//    }
//}