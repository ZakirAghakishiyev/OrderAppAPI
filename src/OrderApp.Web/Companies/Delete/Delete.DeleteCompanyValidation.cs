using FluentValidation;

namespace OrderApp.Web.Companies.Delete;

public class DeleteCompanyValidator : Validator<DeleteCompanyRequest>
{
  public DeleteCompanyValidator()
  {
    RuleFor(x => x.CompanyId)
      .GreaterThan(0);
  }
}