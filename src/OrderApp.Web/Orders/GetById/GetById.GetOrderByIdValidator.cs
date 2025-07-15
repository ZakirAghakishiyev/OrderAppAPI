using FluentValidation;

namespace OrderApp.Web.Orders.GetById;

public class GetOrderByIdValidator : Validator<GetOrderByIdRequest>
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.Id)
        .GreaterThan(0)
        .WithMessage("Order ID must be greater than 0.");
    }
}
