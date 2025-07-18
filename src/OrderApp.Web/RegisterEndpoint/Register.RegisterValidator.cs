using FluentValidation;

namespace OrderApp.Web.RegisterEndpoint;

public class RegisterValidator : Validator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MaximumLength(50);
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(4)
            .WithMessage("Password must be at least 6 characters long.");
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
    }
}
