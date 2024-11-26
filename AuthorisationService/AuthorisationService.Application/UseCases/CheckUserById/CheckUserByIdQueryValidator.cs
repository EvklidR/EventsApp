using FluentValidation;

namespace AuthorisationService.Application.UseCases.Validators
{
    public class CheckUserByIdQueryValidator : AbstractValidator<CheckUserByIdQuery>
    {
        public CheckUserByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
