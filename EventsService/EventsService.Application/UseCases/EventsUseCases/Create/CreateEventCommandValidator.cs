using EventsService.Application.DTOs;
using EventsService.Application.Validators;
using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator(CreateEventDtoValidator createEventDtoValidator)
        {
            RuleFor(x => x.EventDto)
                .NotNull().WithMessage("Event data must not be null.")
                .SetValidator(createEventDtoValidator);

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Image file must not be empty if provided.");
        }
    }
}
