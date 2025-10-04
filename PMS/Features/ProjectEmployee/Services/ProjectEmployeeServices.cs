
using PMS.Domains;
using PMS.Features.Master.Respositories;
using PMS.Features.Master.Services;
using PMS.Features.ProjectEmployee.Repositories;
using PMS.Features.ProjectEmployee.ViewModels;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.ProjectEmployee.Services
{
    public class ProjectEmployeeServices : IProjectEmployeeServices
    {
        private readonly IProjectEmployeeRepository _projectEmployeeRepository;
        private readonly IDepartmentService _departmentService;
        private readonly IDesignationService _designationService;

        public ProjectEmployeeServices(IProjectEmployeeRepository projectEmployeeRepository, IDesignationService designationService, IDepartmentService departmentService)
        {
            _projectEmployeeRepository = projectEmployeeRepository;
            _designationService = designationService;
            _departmentService = departmentService;
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeViewModel> models)> GetMappedProjectEmployee(int projectId)
        {
            var response = await _projectEmployeeRepository.GetMappedEmployee(projectId);

            var departmentModels = await _departmentService.GetDepartments(default);

            var designationModels = await _designationService.GetAllDesignation(default);

            var responseModels=response.MappedProjectEmployee.ToList().Select(x => new EmployeeViewModel()
            {
                Id= x.Id,
                Name= x.Name,
                DepartmentName= departmentModels.Item3.FirstOrDefault(dept=> dept.Id==x.DepartmentId)?.Name ?? string.Empty,
                EmailId= x.EmailId,
                EmployeeCode= x.EmployeeCode,
                DesignationName= designationModels.model.FirstOrDefault(design=> design.Id==x.DesignationId)?.Name?? string.Empty,
            }).ToList();

            return ("Mapped Employee Fetched", true, responseModels);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeViewModel> models)> GetUnMappedProjectEmployee(int projectId)
        {
            var response = await _projectEmployeeRepository.GetUnMappedEmployee(projectId);

            var departmentModels = await _departmentService.GetDepartments(default);

            var designationModels = await _designationService.GetAllDesignation(default);

            var responseModels = response.UnMappedProjectEmployee.ToList().Select(x => new EmployeeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                DepartmentName = departmentModels.Item3.FirstOrDefault(dept => dept.Id == x.DepartmentId)?.Name ?? string.Empty,
                EmailId = x.EmailId,
                EmployeeCode = x.EmployeeCode,
                DesignationName = designationModels.model.FirstOrDefault(design => design.Id == x.DesignationId)?.Name ?? string.Empty,
            }).ToList();

            return ("Un Mapped Employee Fetched", true, responseModels);
        }

        public async Task<(string message, bool isSuccess)> ProjectEmployeeMapping(int projectId, int employeeId, bool isMapped)
        {
            return await _projectEmployeeRepository.ProjectEmployeeMapping(projectId, employeeId, isMapped);
        }
    }
}
