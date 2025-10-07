namespace PMS.Features.UserManagement.ViewModels
{
    public class ChangePasswordVm
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
