using AuthorisationService.Application.Validators;
using FluentValidation;

namespace AuthorisationService.Application.UseCases.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(CreateUserDtoValidator createUserDtoValidator)
        {
            RuleFor(x => x.CreateUserDto)
                .NotNull().WithMessage("User data must not be null.")
                .SetValidator(createUserDtoValidator);
        }
    }
}
