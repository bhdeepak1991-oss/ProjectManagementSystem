using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Services
{
    public interface IUserService
    {
        Task<(string message, bool isSuccess)> CreateUser(Domain.UserManagement model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateUser(Domain.UserManagement model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteUser(int userId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Domain.UserManagement model)> GetUserById(int userId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<UserViewModel> models)> GetUserList(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> LockedUser(int userId, CancellationToken cancellationToken);

        Task<Domain.UserManagement> Authenticate(Domain.UserManagement model);

        Task<(string message, bool isSuccess)> ChangesPassword(ChangePasswordVm model);
    }
}
