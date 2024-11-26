using FluentValidation;

namespace EventsService.Application.UseCases.ParticipantsUseCases.Validators
{
    public class GetParticipantByIdQueryValidator : AbstractValidator<GetParticipantByIdQuery>
    {
        public GetParticipantByIdQueryValidator()
        {
            RuleFor(x => x.ParticipantId)
                .GreaterThan(0).WithMessage("Participant ID must be greater than 0.");
        }
    }
}
