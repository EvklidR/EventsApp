using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EventsService.Application.DTOs;

namespace EventsService.API.Filters
{
    public class ValidateCreateEventDtoAttribute : ActionFilterAttribute
    {
        private readonly IValidator<CreateEventDto> _validator;

        public ValidateCreateEventDtoAttribute(IValidator<CreateEventDto> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("createEventDto", out var value) && value is CreateEventDto createEventDto)
            {
                var validationResult = _validator.Validate(createEventDto);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                }
            }
        }
    }
}
