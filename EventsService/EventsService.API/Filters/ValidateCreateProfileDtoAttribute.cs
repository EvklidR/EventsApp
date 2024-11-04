using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EventsService.Application.DTOs;

namespace EventsService.API.Filters
{
    public class ValidateCreateProfileDtoAttribute : ActionFilterAttribute
    {
        private readonly IValidator<CreateProfileDto> _validator;

        public ValidateCreateProfileDtoAttribute(IValidator<CreateProfileDto> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("profile", out var value) && value is CreateProfileDto createProfileDto)
            {
                var validationResult = _validator.Validate(createProfileDto);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                }
            }
        }
    }
}
