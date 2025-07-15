using FluentValidation;

namespace OrderApp.Web.Products;

public class UpdateProductValidator : Validator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than 0.");

        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("Company ID must be greater than 0.");
    }
}
