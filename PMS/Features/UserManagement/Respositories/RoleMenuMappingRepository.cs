using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Respositories
{
    public class RoleMenuMappingRepository : IRoleMenuMappingRepository
    {
        private readonly PmsDbContext _dbContext;

        public RoleMenuMappingRepository(PmsDbContext context)
        {
            _dbContext = context;
        }
        public async Task<(string message, bool isSuccess)> CreateMappedMenu(RoleMenuMappingVm model, int roleId)
        {
            var mappedMenuModels = await _dbContext.RoleMenuMappings
                            .FirstOrDefaultAsync(x => x.IsActive == true
                                && x.IsDeleted == false && x.RoleId == roleId && x.MenuId == model.Id);

            if (mappedMenuModels is not null)
            {
                mappedMenuModels.IsActive = false;
                mappedMenuModels.IsDeleted = true;

                _dbContext.RoleMenuMappings.Update(mappedMenuModels);
            }

            if (model.IsMapped)
            {

                var updateMenuMapping = new RoleMenuMapping()
                {
                    RoleId = roleId,
                    MenuId = model.Id,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                await _dbContext.AddAsync(updateMenuMapping);
            }

            await _dbContext.SaveChangesAsync();

            return ("Role Menu mapping", true);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<RoleMenuMappingVm> models)> GetMappedMenu(int roleId)
        {
            var menuModels = await _dbContext.MenuMasters
                    .Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();

            var mappedMenuModels = await _dbContext.RoleMenuMappings
                            .Where(x => x.IsActive == true && x.IsDeleted == false && x.RoleId == roleId).ToListAsync();

            var response = menuModels.Select(x => new RoleMenuMappingVm()
            {
                MenuName = x.MenuName,
                SubMenuName = x.SubMenuName,
                IsMapped = mappedMenuModels.Any(z => z.MenuId == x.Id),
                Id = x.Id
            }).ToList();

            return ("Fetched Mapped Menu Details", true, response);
        }
    }
}
