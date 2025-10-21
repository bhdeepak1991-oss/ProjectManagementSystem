using PMS.Features.Dashboard.ViewModels;
using PMS.Features.ProjectTask.ViewModels;

namespace PMS.Features.Dashboard.Services
{
    public interface IDashboardService
    {
        Task<DashboardVm> GetDashboradDetails(int projectId);
        Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskList(int projectId, string taskStatus, CancellationToken cancellationToken);

        Task<IEnumerable<ProjectTaskViewModel>> GetEmpProjectTaskList(int projectId, string taskStatus, string empCode, CancellationToken cancellationToken);
    }
}
