using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Companies;

public class Update : Endpoint<UpdateCompanyRequest, UpdateCompanyResponse>
{
    private readonly AppDbContext _context;

    public Update(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put(UpdateCompanyRequest.Route);
        AllowAnonymous();
        Validator<UpdateCompanyValidator>();
    }

    public override async Task HandleAsync(UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FindAsync(new object[] { request.CompanyId }, cancellationToken);
        if (company == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        company.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);

        var response = new UpdateCompanyResponse(new CompanyRecord(company.Id, company.Name));
        await SendAsync(response);
    }
}
