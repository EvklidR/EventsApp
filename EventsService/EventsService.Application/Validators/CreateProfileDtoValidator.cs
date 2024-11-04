using EventsService.Application.DTOs;
using FluentValidation;

namespace EventsService.Application.Validators
{
    public class CreateProfileDtoValidator : AbstractValidator<CreateProfileDto>
    {
        public CreateProfileDtoValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");
            ;

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(1, 100).WithMessage("Surname must be between 1 and 100 characters.");
            ;

            RuleFor(x => x.DateOfBirthday)
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
