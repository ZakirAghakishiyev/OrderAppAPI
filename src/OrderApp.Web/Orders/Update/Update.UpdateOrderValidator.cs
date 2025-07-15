namespace OrderApp.Web.Orders.Update;

using FluentValidation;

public class UpdateOrderValidator : Validator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
        RuleFor(o => o.OrderDate)
            .NotEmpty()
            .WithMessage("Order date is required.");
        RuleFor(o => o.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than zero.");
        RuleFor(o => o.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.");
    }
}
