using FluentValidation;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordVm>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(5).WithMessage("Password must be at least 5 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");

        }
    }
}
