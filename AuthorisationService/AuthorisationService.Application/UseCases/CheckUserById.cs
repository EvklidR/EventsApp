using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.Interfaces.UseCases;
using AuthorisationService.Application.Models;
using AuthorisationService.Domain.Interfaces;

namespace AuthorisationService.Application.UseCases
{
    public class CheckUserById : ICheckUserById
    {
        private readonly IUserRepository _userRepository;

        public CheckUserById(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> ExecuteAsync(int id)
        {
            var existUser = await _userRepository.GetByIdAsync(id);
            return existUser != null;
        }

    }
}
