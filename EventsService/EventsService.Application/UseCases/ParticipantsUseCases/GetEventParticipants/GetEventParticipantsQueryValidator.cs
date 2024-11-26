using FluentValidation;

namespace EventsService.Application.UseCases.ParticipantsUseCases.Validators
{
    public class GetEventParticipantsQueryValidator : AbstractValidator<GetEventParticipantsQuery>
    {
        public GetEventParticipantsQueryValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event ID must be greater than 0.");
        }
    }
}
