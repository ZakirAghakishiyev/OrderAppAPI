using FluentValidation;

namespace OrderApp.Web.Products.GetById;

public class GetProductByIdValidator:Validator<GetProductByIdRequest>
{
    public GetProductByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0.");
    }
}
