using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Companies;

public class List : EndpointWithoutRequest<CompanyListResponse>
{
    private readonly AppDbContext _context;

    public List(AppDbContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Get("/Companies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var company = await _context.Companies.ToListAsync();
        Response = new CompanyListResponse
        {
            Companies = company.Select(c => new CompanyRecord(c.Id, c.Name)).ToList()
        };
    }
}
