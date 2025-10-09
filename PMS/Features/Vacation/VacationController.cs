using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Features.Project.Services;
using PMS.Features.Vacation.Services;
using PMS.Helpers;

namespace PMS.Features.Vacation
{
    public class VacationController : Controller
    {
        private readonly IVacationService _vacationService;
        private readonly string viewName = "~/Features/Vacation/Views/";
        private readonly IProjectService _projectService;
        public VacationController(IVacationService vacationService, IProjectService projectService)
        {
            _vacationService = vacationService;
            _projectService = projectService;
        }

        public async Task<IActionResult> CreateVacation(int id)
        {
            var responseModel = await _vacationService.GetVacationModel(id, default);

            var projectModel = await _projectService.GetProjectList(default);

            ViewBag.Project = new SelectList(projectModel.model, "Id", "Name");

            return View($"{viewName}CreateVacation.cshtml", responseModel.model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVacation(Domains.VacationDetail model)
        {
            if (model.Id == 0)
            {
                model.CreatedBy = Convert.ToInt32(HttpContext.GetEmployeeId());

                var responseModel = await _vacationService.CreateVacation(model, default);

                return Json(responseModel);
            }

            model.UpdatedBy = Convert.ToInt32(HttpContext.GetEmployeeId());

            var updateResponse = await _vacationService.UpdateVacation(model, default);

            return Json(updateResponse);
        }

        public async Task<IActionResult> GetVacationList()
        {
            var responseModels = await _vacationService.GetVacationDetail(default);

            return PartialView($"{viewName}VacationList.cshtml", responseModels.models);
        }

        public async Task<IActionResult> DeleteVacation(int id)
        {
            var response = await _vacationService.DeleteVacationById(id, default);

            return Json(response);
        }
    }
}
