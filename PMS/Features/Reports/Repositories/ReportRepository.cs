
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using System.Data;
using System.Data.SqlClient;

namespace PMS.Features.Reports.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IConfiguration _configuration;
        private readonly PmsDbContext _dbContext;

        public ReportRepository(IConfiguration configuration, PmsDbContext pmsDbContext)
        {
            _configuration = configuration;
            _dbContext = pmsDbContext;
        }

        public async  Task<IFormFile> GetReportDetail(string reportName)
        {
            var response = await _dbContext.ReportMasters.FirstOrDefaultAsync(x => x.ReportName == reportName);
            var dt = new DataTable();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            using (var cmd = new SqlCommand($"exec {response.ReportQuery}", conn))
            {
                cmd.Parameters.AddWithValue("@ReportName", reportName);
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Report");
            ws.Cell(1, 1).InsertTable(dt);

            
            var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "report", response.ReportDisplayName + ".xlsx")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            return await Task.FromResult(file);

        }

        public async Task<IEnumerable<ReportMaster>> GetReportList()
        {
            var response= await _dbContext.ReportMasters.Where(x=>x.IsActive==true && x.IsDeleted==false).ToListAsync();
            return response;
        }
    }
}
