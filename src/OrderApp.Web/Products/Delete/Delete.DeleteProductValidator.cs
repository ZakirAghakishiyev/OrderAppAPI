namespace OrderApp.Web.Products;

using FluentValidation;

public class DeleteProductValidator:Validator<DeleteProductRequest>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0.");
    }
}
