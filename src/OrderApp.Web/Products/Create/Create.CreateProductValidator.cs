namespace OrderApp.Web.Products;

using FluentValidation;

public class CreateProductValidator : Validator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than zero.");
    }
}
