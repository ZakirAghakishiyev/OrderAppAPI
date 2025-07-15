using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Products;

public class Delete : Endpoint<DeleteProductRequest>
{
    private readonly AppDbContext _context;

    public Delete(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(DeleteProductRequest.Route);
        AllowAnonymous();
        Validator<DeleteProductValidator>();
        Description(x => x
            .WithName("Delete Product")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound));
    }
    public override async Task HandleAsync(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        if (product == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);

        await SendAsync($"Product with id {product.Id} deleted");
    }
}
