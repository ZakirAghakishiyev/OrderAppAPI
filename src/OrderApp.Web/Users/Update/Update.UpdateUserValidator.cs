using FluentValidation;

namespace OrderApp.Web.Users.Update;

public class UpdateUserValidator:Validator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100);
        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(4)
            .WithMessage("Length should be greater than 4");
    }
}
