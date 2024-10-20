using EventsApp.EventsService.Application.DTOs;
using FluentValidation;

namespace EventsApp.EventsService.Application.Validators
{
    public class ParticipantOfEventDtoValidator : AbstractValidator<ParticipantOfEventDto>
    {
        public ParticipantOfEventDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.DateOfBirthday)
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past.");
        }
    }
}
