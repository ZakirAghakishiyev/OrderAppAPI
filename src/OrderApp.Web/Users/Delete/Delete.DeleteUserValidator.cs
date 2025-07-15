using FluentValidation;

namespace OrderApp.Web.Users.Delete;

public class DeleteUserValidator : Validator<DeleteUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");
    }
}
