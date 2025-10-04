
using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.ProjectEmployee.Repositories
{
    public class ProjectEmployeeRepository : IProjectEmployeeRepository
    {
        private readonly PmsDbContext _dbContext;

        public ProjectEmployeeRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<Employee> MappedProjectEmployee)> GetMappedEmployee(int projectId)
        {
            var projectEmployee = await _dbContext.ProjectEmployees.AsNoTracking()
                .Where(x => x.ProjectId == projectId && x.IsDeleted == false).Select(x=>x.EmployeeId).ToListAsync();

            var employees = await _dbContext.Employees.AsNoTracking()
                .Where(x => projectEmployee.Contains(x.Id) && x.IsDeleted == false).ToListAsync();

            return ("Mapped Employee for Project", true, employees);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<Employee> UnMappedProjectEmployee)> GetUnMappedEmployee(int projectId)
        {
            var projectEmployee = await _dbContext.ProjectEmployees.AsNoTracking()
               .Where(x => x.ProjectId == projectId && x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();

            var employeesNotInProject = await _dbContext.Employees.AsNoTracking()
                        .Where(x => !projectEmployee.Contains(x.Id) && x.IsDeleted == false).ToListAsync();

            return ("UnMapped Employee for Project", true, employeesNotInProject);
        }

        public async Task<(string message, bool isSuccess)> ProjectEmployeeMapping(int projectId, int employeeId, bool isMapped)
        {
            if (isMapped)
            {
                await _dbContext.ProjectEmployees.AddAsync(new Domains.ProjectEmployee
                {
                    EmployeeId = employeeId,
                    ProjectId = projectId,
                    CreatedBy = 1, // TODO: Need to change
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                });

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var dbModel = await _dbContext.ProjectEmployees.
                        FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.ProjectId == projectId && x.IsDeleted == false);

                if (dbModel is not null)
                {

                    dbModel.IsDeleted = true;
                    dbModel.UpdatedDate = DateTime.Now;

                    _dbContext.ProjectEmployees.Update(dbModel);

                    await _dbContext.SaveChangesAsync();
                }
            }

            return ("Project employee Mapped", true);
        }
    }
}
