using FluentValidation;

namespace OrderApp.Web.Companies;

public class UpdateCompanyValidator : Validator<UpdateCompanyRequest>
{
  public UpdateCompanyValidator()
  {
    RuleFor(c=>c.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MaximumLength(100);
  }
}
