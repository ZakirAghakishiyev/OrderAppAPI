using FluentValidation;

namespace OrderApp.Web.Orders.Create;

public class CreateOrderValidator:AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderDate)
            .NotEmpty()
            .WithMessage("Order date is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than zero.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.");
    }
}
