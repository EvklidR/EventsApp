using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class GetEventNameIdQueryValidator : AbstractValidator<GetEventByIdQuery>
    {
        public GetEventNameIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");
        }
    }
}
