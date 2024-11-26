using FluentValidation;

namespace EventsService.Application.UseCases.ParticipantsUseCases.Validators
{
    public class UnregisterUserFromEventCommandValidator : AbstractValidator<UnregisterUserFromEventCommand>
    {
        public UnregisterUserFromEventCommandValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        }
    }
}
