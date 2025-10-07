using Microsoft.AspNetCore.Mvc;
using PMS.Domains;
using PMS.Features.EmployeeTimeSheet.Services;
using PMS.Features.EmployeeTimeSheet.ViewModels;
using PMS.Helpers;

namespace PMS.Features.EmployeeTimeSheet
{
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
                 models= response.Item3.ToList()
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
                    IsActive=true,
                    IsDeleted=false,
                    CreatedDate= DateTime.Now,
                    CreatedBy= Convert.ToInt32(HttpContext.GetUserId())
                };

                timeSheetModel.Add(model);
            }

            var response = await _timeSheetService.UploadTimeSheet(timeSheetModel, default);

            return Json(response);
        }
    }
}
