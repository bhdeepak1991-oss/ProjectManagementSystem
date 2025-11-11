using PMS.Domains;

namespace PMS.Features.Reports.Repositories
{
    public interface IReportRepository
    {
        Task<IFormFile> GetReportDetail(string reportName);
        Task<IEnumerable<ReportMaster>> GetReportList();
    }
}
