using MediatR;
using AuthorisationService.Application.Models;
using AuthorisationService.Application.DTOs;

namespace AuthorisationService.Application.UseCases
{
    public class RegisterUserCommand : IRequest<AuthenticatedResponse>
    {
        public CreateUserDto CreateUserDto { get; set; }

        public RegisterUserCommand(CreateUserDto createUserDto)
        {
            CreateUserDto = createUserDto;
        }
    }
}
