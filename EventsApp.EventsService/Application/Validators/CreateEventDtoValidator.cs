using EventsApp.EventsService.Application.DTOs;
using FluentValidation;

namespace EventsApp.EventsService.Application.Validators
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Event name is required.")
                .Length(1, 100).WithMessage("Event name must be between 1 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Event description is required.")
                .Length(1, 500).WithMessage("Event description must be between 1 and 500 characters.");

            RuleFor(x => x.DateTimeHolding)
                .GreaterThan(DateTime.Now).WithMessage("Event date must be in the future.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Event location is required.");

            RuleFor(x => x.MaxParticipants)
                .GreaterThan(0).WithMessage("Max participants must be greater than zero.");
        }
    }
}
