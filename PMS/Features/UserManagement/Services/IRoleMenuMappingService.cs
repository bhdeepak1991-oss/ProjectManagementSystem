using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Services
{
    public interface IRoleMenuMappingService
    {
        Task<(string message, bool isSuccess, IEnumerable<RoleMenuMappingVm> models)> GetMappedMenu(int roleId);
        Task<(string message, bool isSuccess)> CreateMappedMenu(RoleMenuMappingVm model, int roleId);
    }
}
