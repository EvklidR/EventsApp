using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
    {
        public DeleteEventCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");
        }
    }
}
