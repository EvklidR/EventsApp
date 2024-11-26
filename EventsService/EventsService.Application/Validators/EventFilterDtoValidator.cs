using EventsService.Application.DTOs;
using FluentValidation;

namespace EventsService.Application.Validators
{
    public class EventFilterDtoValidator : AbstractValidator<EventFilterDto>
    {
        public EventFilterDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");

            RuleFor(x => x.Date)
                .GreaterThanOrEqualTo(DateTime.MinValue).When(x => x.Date.HasValue)
                .WithMessage("Invalid date value.");

            RuleFor(x => x.Location)
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Location));

            RuleFor(x => x.Category)
                .IsInEnum().When(x => x.Category.HasValue)
                .WithMessage("Invalid event category.");
        }
    }
}
