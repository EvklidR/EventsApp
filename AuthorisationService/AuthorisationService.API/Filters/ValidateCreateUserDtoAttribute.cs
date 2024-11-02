using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using AuthorisationService.Application.DTOs;

namespace AuthorisationService.Api.Filters
{
    public class ValidateCreateUserDtoAttribute : ActionFilterAttribute
    {
        private readonly IValidator<CreateUserDto> _validator;

        public ValidateCreateUserDtoAttribute(IValidator<CreateUserDto> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("createUserDto") &&
                context.ActionArguments["createUserDto"] is CreateUserDto createUserDto)
            {
                var validationResult = _validator.Validate(createUserDto);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                }
            }
        }
    }
}
