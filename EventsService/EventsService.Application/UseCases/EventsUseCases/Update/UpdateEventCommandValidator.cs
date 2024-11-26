using EventsService.Application.DTOs;
using EventsService.Application.Validators;
using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator(UpdateEventDtoValidator updateEventDtoValidator)
        {
            RuleFor(x => x.UpdateEventDto)
                .NotNull().WithMessage("Update event data must not be null.")
                .SetValidator(updateEventDtoValidator);
        }
    }
}
