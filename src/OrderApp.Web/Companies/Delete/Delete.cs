using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Companies.Delete;

public class Delete : Endpoint<DeleteCompanyRequest>
{
    private readonly AppDbContext _context;

    public Delete(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(DeleteCompanyRequest.Route);
        AllowAnonymous();
        Validator<DeleteCompanyValidator>();
    }

    public override async Task HandleAsync(DeleteCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.CompanyId);
        if (company == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync(cancellationToken);

        await SendAsync($"User with id {company.Id} deleted");
    }
}
