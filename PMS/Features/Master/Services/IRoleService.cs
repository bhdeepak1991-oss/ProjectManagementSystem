using PMS.Domains;

namespace PMS.Features.Master.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleMaster>> GetRoles(CancellationToken cancellationToken);
        Task<bool> CreateRole(RoleMaster model, CancellationToken cancellationToken);
        Task<RoleMaster> GetRoleById(int roleId, CancellationToken cancellationToken);
        Task<bool> UpdateRole(RoleMaster model, CancellationToken cancellationToken);
        Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken);
    }
}
