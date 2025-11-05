using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.AccessManagement.ViewModels;

namespace PMS.Features.AccessManagement.Repositories
{
    public class AccessManagementRepository : IAccessManagementRepository
    {
        private readonly PmsDbContext _dbContext;

        public AccessManagementRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(string message, bool isSuccess)> CreateMenuSubmenu(MenuMaster model, CancellationToken cancellationToken)
        {
            var response = await _dbContext.MenuMasters.AddAsync(model, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Menu Submenu created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteMenuMaster(int id, CancellationToken cancellationToken)
        {
            var menuMaster = await _dbContext.MenuMasters.FindAsync(id, cancellationToken);

            if (menuMaster is null)
            {
                return ($"Menu Master not found for Id {id}", false);
            }

            menuMaster.IsDeleted = true;
            menuMaster.IsActive = false;
            menuMaster.UpdatedDate = DateTime.UtcNow;
            menuMaster.UpdatedBy = 1; // TODO: Need to change

            _dbContext.MenuMasters.Update(menuMaster);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Menu Master deleted successfully !", true);
        }

        public async Task<(string message, bool isSuccess, MenuMaster model)> GetMenuSubmenuById(int id, CancellationToken cancellationToken)
        {
            var response = await _dbContext.MenuMasters.FindAsync(id, cancellationToken);

            return ("Menu Submenu fetched successfully", true, response ?? new MenuMaster());
        }

        public async Task<(string message, bool isSuccess, IEnumerable<AccesModel> models)> GetMenuSubmenuByRole(int roleId, CancellationToken cancellationToken)
        {
            var response =await  (from rm in _dbContext.RoleMenuMappings
                                    join um in _dbContext.UserManagements on rm.RoleId equals um.RoleId
                                    join mm in _dbContext.MenuMasters on rm.MenuId equals mm.Id
                                    where rm.IsActive == true
                                    && rm.IsDeleted == false
                                    && um.IsDeleted == false
                                    && um.IsLocked == false
                                    && rm.RoleId == roleId
                            orderby mm.DisplayOrder, rm.DisplayOrder
                                    select new AccesModel
                                    {
                                        ActionName = mm.ActionName ?? string.Empty,
                                        ControllerName= mm.ControllerName ?? string.Empty,
                                        MenuName= mm.MenuName ?? string.Empty,
                                        SubmenuName= mm.SubMenuName ?? string.Empty,
                                        IconClass= mm.IconClass ?? string.Empty
                                    }).ToListAsync();

            return ("Menu Submenu fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<MenuMaster> models)> GetMenuSubmenuList(CancellationToken cancellationToken)
        {
            var response = await _dbContext.MenuMasters.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync(cancellationToken);

            return ("Menu Submenu fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess)> UpdateMenuSubmenu(MenuMaster model, CancellationToken cancellationToken)
        {
            var updateModel = _dbContext.MenuMasters.Update(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Menu Submenu updated Successfully !", true);
        }
    }
}
