using System.Data;
using FluentValidation;

namespace OrderApp.Web.Orders.Delete;

public class DeleteOrderValidator : AbstractValidator<DeleteOrderRequest>
{
    public DeleteOrderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id Should be greater than 0");
    }
}
