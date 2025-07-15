using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Products;

public class List : EndpointWithoutRequest<ProductListResponse>
{
    private readonly AppDbContext _context;

    public List(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/Products");
        AllowAnonymous();
        Description(x => x
            .WithName("Get Products")
            .Produces<ProductListResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .Include(x=> x.Company)
            .ToListAsync();

        if (products == null || !products.Any())
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        Response = new ProductListResponse { 
            Products = products.Select(x => new ProductRecord(x.Id, x.Name, x.Price, x.CompanyId, x.Company)).ToList()
        };
        await SendOkAsync(Response, cancellationToken);
    }
}
