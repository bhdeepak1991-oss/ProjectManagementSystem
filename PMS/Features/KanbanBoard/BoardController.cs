using Microsoft.AspNetCore.Mvc;
using PMS.Attributes;
using PMS.Features.KanbanBoard.Services;
using PMS.Helpers;
using System.Threading.Tasks;

namespace PMS.Features.KanbanBoard
{

    [PmsAuthorize]
    public class BoardController : Controller
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public async Task<IActionResult> Index()
        {
            var empId = Convert.ToInt32(HttpContext.GetEmployeeId());
            var projId = Convert.ToInt32(HttpContext.GetProjectId());
            var response = await _boardService.GetBoardDetail(empId, projId);
            return View("~/Features/KanbanBoard/Views/ProjectBoard.cshtml", response);
        }
    }
}
