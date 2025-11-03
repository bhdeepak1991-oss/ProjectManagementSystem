using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Respositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PmsDbContext _dbContext;

        public EmployeeRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> CreateEmployee(Employee empModel, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Employees.AddAsync(empModel);

            var isExists = await _dbContext.Employees.AnyAsync(x => x.EmployeeCode.Trim().ToLower() == empModel.EmployeeCode.Trim().ToLower()
                            || x.EmailId.Trim().ToLower() == empModel.EmailId.Trim().ToLower());

            if (isExists)
            {
                return ("Employee already present", false);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Employee created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteEmployee(int empId, CancellationToken cancellationToken)
        {
            var empModel = await _dbContext.Employees.FindAsync(empId);

            if (empModel is null)
            {
                return ($"Employee with Id {empId} not found", false);
            }

            empModel.IsDeleted = true;

            _dbContext.Employees.Update(empModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ($"Employee with EmpId {empId} deleted successfully", true);
        }

        public async Task<(string message, bool isSuccess, Employee model)> GetEmployeeById(int empId, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == empId);

            return ("Employee fetched successfully", true, response ?? new Employee());
        }

        public async Task<(string message, bool isSuccess, EmployeeVm model)> GetEmployeeDetailById(int empId)
        {
            var employees = await (from emp in _dbContext.Employees
                                   //join um in _dbContext.UserManagements on emp.Id equals um.EmployeeId
                                   join dm in _dbContext.DepartmentMasters on emp.DepartmentId equals dm.Id
                                   join dsm in _dbContext.DesignationMasters on emp.DesignationId equals dsm.Id
                                   where emp.IsDeleted == false && emp.Id== empId
                                   select new EmployeeVm
                                   {
                                       EmployeeName = emp.Name ?? string.Empty,
                                       DepartmentName = dm.Name ?? string.Empty,
                                       DesignationName = dsm.Name ?? string.Empty,
                                       EmployeeCode = emp.EmployeeCode ?? string.Empty,
                                       EmailId = emp.EmailId ?? string.Empty,
                                       DateOfBirth = emp.DateOfBirth,
                                       DateOfJoining = emp.DateOfJoining,
                                       PhoneNumber = emp.PhoneNumber ?? string.Empty,
                                   }).FirstOrDefaultAsync();

            var assignProjects = await (from pe in _dbContext.ProjectEmployees
                                        join pm in _dbContext.Projects on pe.ProjectId equals pm.Id
                                        where pe.EmployeeId == empId
                                        select new ProjectVm
                                        {
                                            ProjectName = pm.Name ?? string.Empty,
                                            ClientName = pm.ClientName ?? string.Empty,
                                            ProjectStatus = pm.ProjectStatus ?? string.Empty,
                                            StartDate = pm.ProjectStartDate,
                                            EndDate = pm.ProjectEndDate
                                        }).ToListAsync();

            if (employees is not null)
            {
                employees.AssignProjectList = assignProjects;
            }

            return ("Employee Fetched successfully", true, employees ?? new());

        }

        public async Task<(string message, bool isSuccess, IEnumerable<Employee> empModels)> GetEmployeeList(CancellationToken cancellationToken)
        {
            var response = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            return ("Employee fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess)> UpdateEmployee(Employee empModel, CancellationToken cancellationToken)
        {
            var dbModel = await _dbContext.Employees.FindAsync(empModel.Id);

            if (dbModel is null)
            {
                return ($"Employee with Id {empModel.Id} not present", false);
            }

            var isExists = await _dbContext.Employees.AnyAsync(x => x.Id != empModel.Id &&
            (x.EmployeeCode == empModel.EmployeeCode || x.EmailId == empModel.EmailId));

            if (isExists)
            {
                return (@$"Employee with EmpCode {empModel.EmployeeCode}
                        and Email Id {empModel.EmailId} already mapped with another employee", false);
            }

            dbModel.Name = empModel.Name;
            dbModel.DepartmentId = empModel.DepartmentId;
            dbModel.DesignationId = empModel.DesignationId;
            dbModel.EmployeeCode = empModel.EmployeeCode;
            dbModel.DateOfBirth = empModel.DateOfBirth;
            dbModel.DateOfJoining = empModel.DateOfJoining;
            dbModel.CreatedBy = empModel.CreatedBy;
            dbModel.CreatedDate = DateTime.Now;

            _dbContext.Employees.Update(dbModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Employee updated successfully", true);
        }
    }
}
