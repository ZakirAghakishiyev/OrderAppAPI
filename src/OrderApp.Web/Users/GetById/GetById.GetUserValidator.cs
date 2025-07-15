using FluentValidation;

namespace OrderApp.Web.Users.GetById;

public class GetUserValidator : Validator<GetUserByIdRequest>
{
    public GetUserValidator()
    {
        RuleFor(x=>x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");   
    }
}
