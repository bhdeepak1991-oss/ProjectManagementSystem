using Microsoft.AspNetCore.Mvc;
using PMS.Domains;
using PMS.Features.Reports.Repositories;

namespace PMS.Features.Reports
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IActionResult> ReportDetail()
        {
            var response = await _reportRepository.GetReportList();
            ViewBag.ReportList = response;
            return View("~/Features/Reports/Views/PmsReport.cshtml");
        }

        
        public async Task<IActionResult> Index(string reportName)
        {
            var response = await _reportRepository.GetReportDetail(reportName);
            return File(response.OpenReadStream(), response.ContentType, response.FileName);
        }
    }
}
