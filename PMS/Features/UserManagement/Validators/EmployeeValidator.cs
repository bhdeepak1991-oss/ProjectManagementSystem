using FluentValidation;
using PMS.Domains;

namespace PMS.Features.UserManagement.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.DepartmentId)
                .NotNull().WithMessage("Department is required.");

            RuleFor(x => x.DesignationId)
                .NotNull().WithMessage("Designation is required.");

            RuleFor(x => x.EmployeeCode)
                .NotEmpty().WithMessage("Employee Code is required.")
                .MaximumLength(20).WithMessage("Employee Code cannot exceed 20 characters.");

            RuleFor(x => x.EmailId)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.DateOfBirth)
                .NotNull().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past.");

            RuleFor(x => x.DateOfJoining)
                .NotNull().WithMessage("Date of Joining is required.")
                .GreaterThan(x => x.DateOfBirth ?? DateTime.MinValue)
                    .WithMessage("Date of Joining must be after Date of Birth.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");
        }
    }
}
