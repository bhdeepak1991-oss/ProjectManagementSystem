using Microsoft.AspNetCore.Mvc;
using PMS.Features.Utilities.Services;
using PMS.Features.Utilities.ViewModels;

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
            return View("~/Features/Utilities/Views/UploadStatusHelper.cshtml");
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
    }
}
