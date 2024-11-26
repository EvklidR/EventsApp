using FluentValidation;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases.Validators
{
    public class NotifyParticipantsCommandValidator : AbstractValidator<NotifyParticipantsCommand>
    {
        public NotifyParticipantsCommandValidator(IValidator<UpdateEventDto> updateEventDtoValidator)
        {
            RuleFor(x => x.UpdateEventDto)
                .NotNull().WithMessage("UpdateEventDto must not be null.")
                .SetValidator(updateEventDtoValidator);
        }
    }
}
