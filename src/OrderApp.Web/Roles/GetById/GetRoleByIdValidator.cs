using FluentValidation;

namespace OrderApp.Web.Roles.GetById;

public class GetRoleByIdValidator:Validator<GetRoleByIdRequest>
{
    public GetRoleByIdValidator()
    {
        RuleFor(r => r.Id)
            .GreaterThan(0);        
    }
}
