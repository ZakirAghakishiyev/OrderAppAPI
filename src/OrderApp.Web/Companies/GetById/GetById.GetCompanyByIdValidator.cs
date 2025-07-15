using FluentValidation;

namespace OrderApp.Web.Companies.GetById;

public class GetCompanyValidator : Validator<GetCompanyByIdRequest>
{
  public GetCompanyValidator()
  {
    RuleFor(x => x.CompanyId)
      .GreaterThan(0);
  }
}