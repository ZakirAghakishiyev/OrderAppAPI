using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Products;

public class Create : Endpoint<CreateProductRequest, CreateProductResponse>
{
    private readonly AppDbContext _context;

    public Create(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post(CreateProductRequest.Route);
        AllowAnonymous();
        Validator<CreateProductValidator>();
        Description(x => x
            .WithName("Create Product")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest));
    }
    public override async Task HandleAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Core.ProductAggregate.Product
        {
            Name = request.Name,
            Price = request.Price,
            CompanyId = request.CompanyId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        Response = new CreateProductResponse(product.Id, product.Name, product.Price, product.CompanyId);
    }

}
