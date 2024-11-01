using EventsService.Application.DTOs;
using FluentValidation;

namespace EventsService.Application.Validators
{
    public class UpdateEventDtoValidator : AbstractValidator<UpdateEventDto>
    {
        public UpdateEventDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");

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
