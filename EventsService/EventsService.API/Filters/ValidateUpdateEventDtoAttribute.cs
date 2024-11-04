using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EventsService.Application.DTOs;

namespace EventsService.API.Filters
{
    public class ValidateUpdateEventDtoAttribute : ActionFilterAttribute
    {
        private readonly IValidator<UpdateEventDto> _validator;

        public ValidateUpdateEventDtoAttribute(IValidator<UpdateEventDto> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("updateEventDto", out var value) && value is UpdateEventDto updateEventDto)
            {
                var validationResult = _validator.Validate(updateEventDto);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                }
            }
        }
    }
}
