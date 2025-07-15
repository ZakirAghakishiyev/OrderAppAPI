using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Products;

public class Update : Endpoint<UpdateProductRequest, UpdateProductResponse>
{
    private readonly AppDbContext _context;
    public Update(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put(UpdateProductRequest.Route);
        AllowAnonymous();
        Validator<UpdateProductValidator>();
        Description(x => x
            .WithName("Update Product")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound));
    }
    public override async Task HandleAsync(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
        .FindAsync(request.Id, cancellationToken)
;
        if (product == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.CompanyId = request.CompanyId;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new UpdateProductResponse(new ProductRecord(product.Id, product.Name, product.Price, product.CompanyId));
        await SendAsync(response);
    }
}
