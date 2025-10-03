using FluentValidation;
using PMS.Domains;

namespace PMS.Features.Master.Validators
{
    public class DepartmentValidator : AbstractValidator<DepartmentMaster>
    {
        public DepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Department Name is required.");
        }
    }
}
