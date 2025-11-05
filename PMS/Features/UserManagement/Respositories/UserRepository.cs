
using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PmsDbContext _dbContext;

        public UserRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.UserManagement> Authenticate(Domain.UserManagement model)
        {
            var responseModel = await _dbContext.UserManagements.FirstOrDefaultAsync(x => x.UserName == model.UserName && x.Password == model.Password);
            return responseModel ?? new Domain.UserManagement();
        }

        public async Task<(string message, bool isSuccess)> ChangesPassword(Domain.UserManagement model)
        {
            var updatePasswordModel = await _dbContext.UserManagements.FirstOrDefaultAsync(x=>x.UserName==model.UserName);

            if (updatePasswordModel is null)
            {
                return ("Record not found", false);
            }

            updatePasswordModel.IsTempPassword = false;
            updatePasswordModel.Password = model.Password;
            updatePasswordModel.UpdatedBy = 1;
            updatePasswordModel.UpdatedDate = DateTime.Now;

            _dbContext.UserManagements.Update(updatePasswordModel);

            await _dbContext.SaveChangesAsync();

            return ("Password updated successfully", true);

        }

        public async Task<(string message, bool isSuccess)> CreateUser(Domain.UserManagement model, CancellationToken cancellationToken)
        {
            var userModel = await _dbContext.UserManagements.AddAsync(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("User created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            var deleteModel = await _dbContext.UserManagements.FindAsync(userId);

            if (deleteModel is null)
            {
                return ("User not found ", false);
            }

            deleteModel.IsDeleted = true;

            deleteModel.UpdatedDate = DateTime.Now;

            _dbContext.UserManagements.Update(deleteModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("User Deleted successfully !", true);
        }

        public async Task<(string message, bool isSuccess, Domain.UserManagement model)> GetUserById(int userId, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.UserManagements.FindAsync(userId, cancellationToken);

            return ("user fetched successfully", true, responseModel ?? new Domain.UserManagement());

        }

        public async Task<(string message, bool isSuccess, IEnumerable<UserViewModel> models)> GetUserList(CancellationToken cancellationToken)
        {

            var response = from um in _dbContext.UserManagements
                           join emp in _dbContext.Employees on um.EmployeeId equals emp.Id
                           join rm in _dbContext.RoleMasters on um.RoleId equals rm.Id
                           join dm in _dbContext.DepartmentMasters on emp.DepartmentId equals dm.Id
                           join dsm in _dbContext.DesignationMasters on emp.DesignationId equals dsm.Id
                           where um.IsDeleted == false && emp.IsDeleted == false
                           select new UserViewModel
                           {
                               Id = um.Id,
                               UserName = um.UserName ?? string.Empty,
                               EmployeeName = emp.Name ?? string.Empty,
                               EmpCode = emp.EmployeeCode ?? string.Empty,
                               RoleName = rm.Name ?? string.Empty,
                               DepartmentName = dm.Name ?? string.Empty,
                               DesignationName = dsm.Name ?? string.Empty,
                               IsLocked = um.IsLocked ?? false,
                               EmailId = emp.EmailId ?? string.Empty
                           };


            return ("User Fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess)> LockedUser(int userId, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.UserManagements.FindAsync(userId, cancellationToken);

            if (responseModel is null)
            {
                return ("User not found", false);
            }

            responseModel.IsLocked = true;

            responseModel.UpdatedDate = DateTime.Now;

            _dbContext.UserManagements.Update(responseModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("User has been locked", true);
        }

        public async Task<(string message, bool isSuccess)> UpdateUser(Domain.UserManagement model, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.UserManagements.FindAsync(model.Id, cancellationToken);

            if (responseModel is null)
            {
                return ("User not found", false);
            }

            responseModel.RoleId = model.RoleId;

            _dbContext.Update(responseModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("user updated successfully", true);
        }
    }
}
