

using Microsoft.AspNetCore.Mvc;
using PMS.Features.Utilities.Services;
using PMS.Features.Utilities.ViewModels;
using PMS.Helpers;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace PMS.Features.Utilities
{
    public class UtilitiesController : Controller
    {
        private readonly IWebHostEnvironment _webHosting;
        private readonly IStatusHelperService _statusHelperService;

        public UtilitiesController(IWebHostEnvironment webHostEnvironment, IStatusHelperService statusHelperService)
        {
            _webHosting = webHostEnvironment;
            _statusHelperService = statusHelperService;
        }
        public IActionResult Index()
        {
            return PartialView("~/Features/Utilities/Views/UploadStatusHelper.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(StatusHelperUploadVm model)
        {
            var response = await _statusHelperService.UploadStatusHelper(model.UploadFile, default);

            return Json(response);
        }
        public async Task<IActionResult> DownloadStausFile()
        {
            var fileName = "UploadStatusFile.xlsx";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFiles", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var memory = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

        public async Task<IActionResult> GetStatusHelper()
        {
            var response = await _statusHelperService.GetStatusHelperDetail(default);
            return PartialView("~/Features/Utilities/Views/StatusTypeList.cshtml", response.models);
        }

        [HttpGet]
        public async Task<IActionResult> TaskUpload()
        {
            return await Task.Run(() => PartialView("~/Features/Utilities/Views/UploadTask.cshtml"));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadTaskTemplate()
        {
            var fileName = "TaskUploadTemplate.xlsx";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFiles", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var memory = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

        [HttpGet]
        public async Task<IActionResult> UploadMaster()
        {
            return await Task.Run(() => View("~/Features/Utilities/Views/Utilities.cshtml"));
        }

        [HttpPost]
        public async Task<IActionResult> UploadTask(StatusHelperUploadVm model)
        {
            int projectId =Convert.ToInt32(HttpContext.GetProjectId());

            var response = await _statusHelperService.UploadTaskHelper(model.UploadFile, projectId, default);

            return Json(new { success= response.isSuccess, models= response.errorResponse, message= response.message});
        }
    }
}
