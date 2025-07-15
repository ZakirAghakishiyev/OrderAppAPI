using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Products.GetById;

public class GetById : Endpoint<GetProductByIdRequest, ProductRecord>
{
    private readonly AppDbContext _context;
    public GetById(AppDbContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Get(GetProductByIdRequest.Route);
        AllowAnonymous();
        Validator<GetProductByIdValidator>();
        Description(x => x
            .WithName("Get Product By Id")
            .Produces<ProductRecord>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound));
    }
    
    public override async Task HandleAsync(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        Response = new ProductRecord(product.Id, product.Name, product.Price, product.CompanyId, product.Company);
        await SendOkAsync(Response, cancellationToken);
    }
}
