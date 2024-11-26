using EventsService.Application.DTOs;
using FluentValidation;

namespace EventsService.Application.UseCases.ParticipantsUseCases.Validators
{
    public class RegisterUserForEventCommandValidator : AbstractValidator<RegisterUserForEventCommand>
    {
        public RegisterUserForEventCommandValidator(IValidator<CreateProfileDto> profileValidator)
        {
            RuleFor(x => x.ProfileDto)
                .NotNull().WithMessage("ProfileDto must not be null.")
                .SetValidator(profileValidator);
        }
    }
}
