namespace PMS.Features.ProjectDocument.Repositories
{
    public interface ITimeSheetRepository
    {
        Task<(string message, bool isSuccess)> CreateTimeSheet(Domains.EmployeeTimeSheet model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.EmployeeTimeSheet>)> GetTimeSheetList(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.EmployeeTimeSheetTaskDetail>)> GetTaskDetail(int timeSheetId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> ApproveDenyTimeSheet(int timeSheetId, bool isApproved, CancellationToken cancellationToken);

    }
}
