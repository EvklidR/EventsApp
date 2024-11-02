using FluentValidation;
using AuthorisationService.Application.DTOs;

namespace AuthorisationService.Application.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required.")
                .Length(3, 50).WithMessage("Login must be between 3 and 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role is invalid.");
        }
    }
}