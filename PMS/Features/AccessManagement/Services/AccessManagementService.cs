using PMS.Domains;
using PMS.Features.AccessManagement.Repositories;
using PMS.Features.AccessManagement.ViewModels;

namespace PMS.Features.AccessManagement.Services
{
    public class AccessManagementService : IAccessManagementService
    {
        private readonly IAccessManagementRepository _accessManagementRepository;

        public AccessManagementService(IAccessManagementRepository accessManagementRepository)
        {
            _accessManagementRepository = accessManagementRepository;
        }
        public async  Task<(string message, bool isSuccess)> CreateMenuSubmenu(MenuMaster model, CancellationToken cancellationToken)
        {
            return await _accessManagementRepository.CreateMenuSubmenu(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteMenuMaster(int id, CancellationToken cancellationToken)
        {
            return await _accessManagementRepository.DeleteMenuMaster(id, cancellationToken);   
        }

        public async Task<(string message, bool isSuccess, MenuMaster model)> GetMenuSubmenuById(int id, CancellationToken cancellationToken)
        {
            return await _accessManagementRepository.GetMenuSubmenuById(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<AccesModel> models)> GetMenuSubmenuByRole(int roleId, CancellationToken cancellationToken)
        {
            return await _accessManagementRepository.GetMenuSubmenuByRole(roleId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<MenuMaster> models)> GetMenuSubmenuList(CancellationToken cancellationToken)
        {
            return await _accessManagementRepository.GetMenuSubmenuList(cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateMenuSubmenu(MenuMaster model, CancellationToken cancellationToken)
        {
            return await _accessManagementRepository.UpdateMenuSubmenu(model, cancellationToken);
        }
    }
}
