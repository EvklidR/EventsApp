using MediatR;
using AuthorisationService.Domain.Interfaces;

namespace AuthorisationService.Application.UseCases
{
    public class CheckUserByIdHandler : IRequestHandler<CheckUserByIdQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public CheckUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CheckUserByIdQuery request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetByIdAsync(request.Id);
            return existUser != null;
        }
    }
}
