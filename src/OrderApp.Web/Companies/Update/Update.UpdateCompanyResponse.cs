namespace OrderApp.Web.Companies;

public class UpdateCompanyResponse(CompanyRecord company)
{
  public CompanyRecord Company { get; set; } = company;
}
