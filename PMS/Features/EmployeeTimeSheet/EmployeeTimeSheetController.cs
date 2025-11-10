using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using PMS.Attributes;
using PMS.Domains;
using PMS.Features.EmployeeTimeSheet.Services;
using PMS.Features.EmployeeTimeSheet.ViewModels;
using PMS.Helpers;

namespace PMS.Features.EmployeeTimeSheet
{

    [PmsAuthorize]
    public class EmployeeTimeSheetController : Controller
    {
        private readonly ITimeSheetService _timeSheetService;

        public EmployeeTimeSheetController(ITimeSheetService timeSheetService)
        {
            _timeSheetService = timeSheetService;
        }

        public IActionResult CreateTimeSheet()
        {
            return View("~/Features/EmployeeTimeSheet/Views/CreateTimeSheet.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimeSheet(Domains.EmployeeTimeSheet model)
        {
            model.EmployeeId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _timeSheetService.CreateTimeSheet(model, default);

            return Json(response);
        }

        public async Task<IActionResult> GetTimeSheet()
        {
            var empId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _timeSheetService.GetTimeSheetList(empId, default);

            return PartialView("~/Features/EmployeeTimeSheet/Views/TimeSheetList.cshtml", response.Item3);
        }

        public async Task<IActionResult> GetTimeSheetDetail(int id)
        {
            var response = await _timeSheetService.GetTimeSheetDetail(id, default);

            var model = new TimeSheetVm()
            {
                models = response.Item3.ToList()
            };

            return View("~/Features/EmployeeTimeSheet/Views/TimeSheetDetail.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTimeSheet()
        {

            var ids = Request.Form["Id"];

            var timeSheetModel = new List<EmployeeTimeSheetTaskDetail>();

            for (int i = 0; i < ids.Count; i++)
            {
                string idValue = ids[i];
                string workingHour = Request.Form["WorkingHour"][i] ?? string.Empty;
                string taskDetail = Request.Form["TaskDetail"][i] ?? string.Empty;
                string dayName = Request.Form["DayName"][i] ?? string.Empty;
                string timeSheetDate = Request.Form["TimeSheetDate"][i] ?? string.Empty;
                string employeeTaskId = Request.Form["EmployeeTimeSheetTaskId"][i] ?? string.Empty;

                var model = new EmployeeTimeSheetTaskDetail
                {
                    Id = Convert.ToInt32(idValue),
                    WorkingHour = workingHour,
                    TaskDetail = taskDetail,
                    DayName = dayName,
                    TimeSheetDate = DateOnly.Parse(timeSheetDate),
                    EmployeeTimeSheetTaskId = Convert.ToInt32(employeeTaskId),
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.GetUserId())
                };

                timeSheetModel.Add(model);
            }

            var response = await _timeSheetService.UploadTimeSheet(timeSheetModel, default);

            return Json(response);
        }

        public async Task<IActionResult> DownloadTimeSheet(int id)
        {
            var response = await _timeSheetService.GetTimeSheetDetail(id, default);

            var timeSheetModel = await _timeSheetService.GetTimeSheetList(Convert.ToInt32(HttpContext.GetEmployeeId()), default);

            var timeSheetName = timeSheetModel.Item3.FirstOrDefault(x => x.Id == id);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Tasks");

                // 3. Add header row
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Time Sheet Date";
                worksheet.Cell(1, 3).Value = "Day Name";
                worksheet.Cell(1, 4).Value = "Working Hour";
                worksheet.Cell(1, 5).Value = "Task Details";

                // 4. Add data rows starting row 2
                int row = 2;
                int count = 1;
                foreach (var item in response.Item3)
                {
                    worksheet.Cell(row, 1).Value = count;
                    worksheet.Cell(row, 2).Value = item.TimeSheetDate?.ToShortDateString();
                    worksheet.Cell(row, 3).Value = item.DayName;
                    worksheet.Cell(row, 4).Value = item.WorkingHour;
                    worksheet.Cell(row, 5).Value = item.TaskDetail;

                    if (item.WorkingHour == "0")
                    {
                        var range = worksheet.Range(row, 1, row, 5); // Row from col 1 to 6
                        range.Style.Fill.BackgroundColor = XLColor.LightPink;

                        if ((item.DayName == "Saturday" || item.DayName == "Sunday") && string.IsNullOrEmpty(item.TaskDetail))
                        {
                            worksheet.Cell(row, 5).Value = "Week Off";
                        }
                    }
                    row++;
                    count++;
                }

                // 5. Format header row (bold + background color)
                var headerRange = worksheet.Range(1, 1, 1, 5);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // 6. Apply table formatting – convert range to “table”
                var dataRange = worksheet.Range(1, 1, row - 1, 5);
                var table = dataRange.CreateTable();
                table.Theme = XLTableTheme.TableStyleMedium9;

                // 7. Auto‑fit columns
                worksheet.Columns().AdjustToContents();

                // 8. Set date column format for DueDate
                worksheet.Column(6).Style.DateFormat.Format = "yyyy‑MM‑dd";

                // 9. Export to MemoryStream then return as file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"EmployeeTimeSheet_{timeSheetName?.TimeSheetName}.xlsx"
                    );
                }
            }
        }
    }
}
