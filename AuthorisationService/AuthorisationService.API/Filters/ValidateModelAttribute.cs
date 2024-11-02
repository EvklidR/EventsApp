using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using FluentValidation.Results;
using AuthorisationService.Application.Models;

namespace AuthorisationService.Api.Filters
{
    public class ValidateModelAttribute : IAsyncActionFilter
    {
        private readonly IValidator<LoginModel> _validator;

        public ValidateModelAttribute(IValidator<LoginModel> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("loginModel", out var value) && value is LoginModel loginModel)
            {
                ValidationResult result = await _validator.ValidateAsync(loginModel);
                if (!result.IsValid)
                {
                    context.Result = new BadRequestObjectResult(result.Errors);
                    return;
                }
            }

            await next();
        }
    }
}
