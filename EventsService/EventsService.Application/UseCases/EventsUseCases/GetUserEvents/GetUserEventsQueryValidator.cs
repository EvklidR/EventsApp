using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class GetUserEventsQueryValidator : AbstractValidator<GetUserEventsQuery>
    {
        public GetUserEventsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        }
    }
}
