using FluentValidation;

namespace OrderApp.Web.Roles.Update;

public class UpdateRoleValidator : Validator<UpdateRoleRequest>
{
    public UpdateRoleValidator()
    {
        
        RuleFor(r => r.Id)
            .GreaterThan(0);
    }
}
