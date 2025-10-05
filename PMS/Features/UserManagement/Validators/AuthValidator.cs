using FluentValidation;

namespace PMS.Features.UserManagement.Validators
{
    public class AuthValidator : AbstractValidator<Domain.UserManagement>
    {
        public AuthValidator()
        {
            RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 5 characters.");
        }
    }
}
