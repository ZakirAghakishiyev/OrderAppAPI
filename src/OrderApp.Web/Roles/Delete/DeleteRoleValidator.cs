using FluentValidation;

namespace OrderApp.Web.Roles.Delete;

public class DeleteRoleValidator : Validator<DeleteRoleRequest>
{
    public DeleteRoleValidator()
    {
        RuleFor(r => r.Id)
            .GreaterThan(0);
    }
}
