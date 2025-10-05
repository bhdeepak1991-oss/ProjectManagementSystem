using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Domains;
using PMS.Features.Master.Services;
using PMS.Features.Sprint.Services;

namespace PMS.Features.Sprint
{
    public class SprintController : Controller
    {
        private readonly ISprintService _sprintService;
        private readonly IDepartmentService _departmentService;

        public SprintController(ISprintService sprintService, IDepartmentService departmentService)
        {
            _sprintService = sprintService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> CreateSprint(int id)
        {
            var responseModel = await _sprintService.GetSprintById(id, default);

            var departmentModel = await _departmentService.GetDepartments(default);

            ViewBag.Departments = new SelectList(departmentModel.Item3, "Id", "Name");

            return View("~/Features/Sprint/Views/CreateSprint.cshtml", responseModel.model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSprint(Domains.Sprint model)
        {
            model.ProjectId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId") ?? 1);

            model.DurationDays = (model.EndDate.Value - model.StartDate.Value).Days;

            if (model.Id != 0)
            {
                var updateResponse = await _sprintService.UpdateSprint(model, default);

                return Json(updateResponse);
            }

            var response = await _sprintService.CreateSprint(model, default);

            return Json(response);
        }

        public async Task<IActionResult> SprintList()
        {
            var response = await _sprintService.GetSprintList(default);

            return PartialView("~/Features/Sprint/Views/SprintList.cshtml", response.models);
        }

        public async Task<IActionResult> GetGoals(int id, string type)
        {
            if (type == "Business")
            {
                ViewBag.goals = await _sprintService.GetSprintBusinessGoal(id, default);
            }
            else
            {
                ViewBag.goals = await _sprintService.GetSprintBusinessGoal(id, default);
            }
            return PartialView("~/Features/Sprint/Views/SprintGoals.cshtml");
        }

        //public async Task<IActionResult> DeleteSprint(int id)
        //{
            
        //}
    }
}
