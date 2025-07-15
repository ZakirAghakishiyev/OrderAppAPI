using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Companies.GetById;

public class GetById : Endpoint<GetCompanyByIdRequest, CompanyRecord>
{
    private readonly AppDbContext _context;

    public GetById(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get(GetCompanyByIdRequest.Route);
        Validator<GetCompanyValidator>();
        AllowAnonymous();
    }

    public override async Task<CompanyRecord> HandleAsync(GetCompanyByIdRequest request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.CompanyId);
        if (company == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return null!;
        }

        Response = new CompanyRecord(company.Id, company.Name);
        return Response;
    }

}
