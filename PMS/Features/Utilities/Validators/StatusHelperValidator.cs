using FluentValidation;
using PMS.Features.Utilities.ViewModels;

namespace PMS.Features.Utilities.Validators
{
    public class StatusHelperValidator : AbstractValidator<StatusHelperUploadVm>
    {
        public StatusHelperValidator()
        {
            RuleFor(x => x.UploadFile)
            .NotNull().WithMessage("Upload file is required.");
            
        }
    }
}
