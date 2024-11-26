using EventsService.Application.DTOs;
using EventsService.Application.Validators;
using FluentValidation;

namespace EventsService.Application.UseCases.EventsUseCases.Validators
{
    public class GetFilteredEventsQueryValidator : AbstractValidator<GetFilteredEventsQuery>
    {
        public GetFilteredEventsQueryValidator(EventFilterDtoValidator eventFilterDtoValidator)
        {
            RuleFor(x => x.Filter)
                .NotNull().WithMessage("Filter data must not be null.")
                .SetValidator(eventFilterDtoValidator);
        }
    }
}
