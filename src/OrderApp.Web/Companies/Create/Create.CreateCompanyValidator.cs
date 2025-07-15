using FluentValidation;

namespace OrderApp.Web.Companies.Create;

public class CreateCompanyValidator : Validator<CreateCompanyRequest>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required.")
        .MaximumLength(100);
    }
}
