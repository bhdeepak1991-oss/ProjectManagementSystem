using PMS.Domains;
using PMS.Features.Master.Respositories;

namespace PMS.Features.Master.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleMasterRepository _roleRepository;

        public RoleService(IRoleMasterRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public  async Task<bool> CreateRole(RoleMaster model, CancellationToken cancellationToken)
        {
            return await _roleRepository.CreateRole(model, cancellationToken);
        }

        public async Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken)
        {
            return await _roleRepository.DeleteRole(roleId, cancellationToken);
        }

        public async Task<RoleMaster> GetRoleById(int roleId, CancellationToken cancellationToken)
        {
            return await _roleRepository.GetRoleById(roleId, cancellationToken);
        }

        public async Task<IEnumerable<RoleMaster>> GetRoles(CancellationToken cancellationToken)
        {
            return await _roleRepository.GetRoles(cancellationToken);
        }

        public async Task<bool> UpdateRole(RoleMaster model, CancellationToken cancellationToken)
        {
            return await _roleRepository.UpdateRole(model, cancellationToken);
        }
    }
}
