using PMS.Features.UserManagement.Respositories;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Services
{
    public class RoleMenuMappingService : IRoleMenuMappingService
    {
        private readonly IRoleMenuMappingRepository _roleMenuMappingRepository;

        public RoleMenuMappingService(IRoleMenuMappingRepository roleMenuMappingRepository)
        {
            _roleMenuMappingRepository = roleMenuMappingRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateMappedMenu(RoleMenuMappingVm model, int roleId)
        {
            return await _roleMenuMappingRepository.CreateMappedMenu(model, roleId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<RoleMenuMappingVm> models)> GetMappedMenu(int roleId)
        {
            return await _roleMenuMappingRepository.GetMappedMenu(roleId);
        }
    }
}
