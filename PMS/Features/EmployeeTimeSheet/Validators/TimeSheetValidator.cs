using FluentValidation;

namespace PMS.Features.EmployeeTimeSheet.Validators
{
    public class TimeSheetValidator : AbstractValidator<Domains.EmployeeTimeSheet>
    {
        public TimeSheetValidator()
        {
            RuleFor(x => x.TimeSheetYear)
                .NotNull().WithMessage("Timesheet year is required.");

            RuleFor(x => x.TimeSheetMonth)
                .NotNull().WithMessage("Timesheet month is required.");

            RuleFor(x => x.TimeSheetName)
                .NotEmpty().WithMessage("Timesheet name is required.")
                .MaximumLength(200).WithMessage("Timesheet name cannot exceed 200 characters.");
        }
    }
}
