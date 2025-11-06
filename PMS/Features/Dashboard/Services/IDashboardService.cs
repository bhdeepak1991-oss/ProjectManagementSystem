using PMS.Features.Dashboard.ViewModels;
using PMS.Features.ProjectTask.ViewModels;

namespace PMS.Features.Dashboard.Services
{
    public interface IDashboardService
    {
        Task<DashboardVm> GetDashboradDetails(int projectId);
        Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskList(int projectId, string taskStatus, CancellationToken cancellationToken);

        Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskTypeList(int projectId, string taskStatus ,string typeOfTask, CancellationToken cancellationToken);
        Task<IEnumerable<ProjectTaskViewModel>> GetEmpProjectTaskList(int projectId, string taskStatus, string empCode, CancellationToken cancellationToken);

        Task<IEnumerable<MasterModelVm>> GetProjectEmployee(int projectId);

        Task<IEnumerable<MasterModelVm>> GetTaskStatus(int projectId);

        Task<IEnumerable<MasterModelVm>> GetTaskType(int projectId);

        Task<IEnumerable<MasterModelVm>> GetTaskPriority(int projectId);
        Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskFilter(int projectId, string taskStatus,
           string priority, string taskType, int empId);

        Task<IEnumerable<PMS.Domains.NotificationDetail>> GetNotificationDetail(int userId);

        Task<IEnumerable<ProjectTaskPercetage>> GetProjectTaskPercetages(int empId);
    }
}
