using PMS.Domains;
using PMS.Features.EmployeeTimeSheet.Repositories;


namespace PMS.Features.EmployeeTimeSheet.Services
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly ITimeSheetRepository _timeSheetRepository;

        public TimeSheetService(ITimeSheetRepository timeSheetRepository)
        {
            _timeSheetRepository = timeSheetRepository;
        }

        public async Task<(string message, bool isSuccess)> ApproveDenyTimeSheet(int timeSheetId, bool isApproved, CancellationToken cancellationToken)
        {
            return await _timeSheetRepository.ApproveDenyTimeSheet(timeSheetId, isApproved, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> CreateTimeSheet(Domains.EmployeeTimeSheet model, CancellationToken cancellationToken)
        {
            return await _timeSheetRepository.CreateTimeSheet(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeTimeSheetTaskDetail>)> GetTimeSheetDetail(int timeSheetId, CancellationToken cancellationToken)
        {
            return await _timeSheetRepository.GetTimeSheetDetail(timeSheetId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<Domains.EmployeeTimeSheet>)> GetTimeSheetList(int empId,CancellationToken cancellationToken)
        {
            return await _timeSheetRepository.GetTimeSheetList(empId,cancellationToken);
        }

        public async  Task<(string message, bool isSuccess)> UploadTimeSheet(List<EmployeeTimeSheetTaskDetail> models, CancellationToken cancellationToken)
        {
            return await _timeSheetRepository.UploadTimeSheet(models, cancellationToken);
        }
    }
}
