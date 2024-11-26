using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class GetEventByNameQueryValidator : AbstractValidator<GetEventByNameQuery>
    {
        public GetEventByNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Event name is required.")
                .Length(1, 100).WithMessage("Event name must be between 1 and 100 characters.");
        }
    }
}
