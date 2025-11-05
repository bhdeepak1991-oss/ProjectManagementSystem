using PMS.Domains;
using PMS.Features.AccessManagement.ViewModels;

namespace PMS.Features.AccessManagement.Services
{
    public interface IAccessManagementService
    {
        Task<(string message, bool isSuccess)> CreateMenuSubmenu(MenuMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateMenuSubmenu(MenuMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, MenuMaster model)> GetMenuSubmenuById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<MenuMaster> models)> GetMenuSubmenuList(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteMenuMaster(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<AccesModel> models)> GetMenuSubmenuByRole(int roleId, CancellationToken cancellationToken);
    }
}
