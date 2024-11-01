using AuthorisationService.Application.Interfaces;
using AuthorisationService.Domain.Entities;
using AuthorisationService.Domain.Interfaces;

namespace AuthorisationService.Application.UseCases
{
    public class GetUserById : IGetUserById
    {
        private readonly IUserRepository _userRepository;

        public GetUserById(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetAsync(u => u.Id == id);
        }
    }

}
