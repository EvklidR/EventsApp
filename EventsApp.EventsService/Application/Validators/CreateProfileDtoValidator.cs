using EventsApp.EventsService.Application.DTOs;
using FluentValidation;

namespace EventsApp.EventsService.Application.Validators
{
    public class CreateProfileDtoValidator : AbstractValidator<CreateProfileDto>
    {
        public CreateProfileDtoValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.");

            RuleFor(x => x.DateOfBirthday)
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
