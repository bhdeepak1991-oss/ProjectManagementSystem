using PMS.Features.KanbanBoard.ViewModels;

namespace PMS.Features.KanbanBoard.Services
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardVm>> GetBoardDetail(int empId, int projectId);
    }
}
