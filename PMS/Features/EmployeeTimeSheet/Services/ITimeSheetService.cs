namespace PMS.Features.EmployeeTimeSheet.Services
{
    public interface ITimeSheetService
    {
        Task<(string message, bool isSuccess)> CreateTimeSheet(Domains.EmployeeTimeSheet model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.EmployeeTimeSheet>)> GetTimeSheetList(int empId,CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.EmployeeTimeSheetTaskDetail>)> GetTimeSheetDetail(int timeSheetId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> ApproveDenyTimeSheet(int timeSheetId, bool isApproved, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UploadTimeSheet(List<Domains.EmployeeTimeSheetTaskDetail> models, CancellationToken cancellationToken);
    }
}
