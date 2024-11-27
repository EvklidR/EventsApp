using MediatR;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Application.Exceptions;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Application.UseCases
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IUserRepository _userRepository;

        public RevokeTokenHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null) throw new Unauthorized("User not found");

            user.RefreshToken = null;
            await _userRepository.CompleteAsync();

        }
    }
}
