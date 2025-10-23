using PMS.Features.KanbanBoard.ViewModels;

namespace PMS.Features.KanbanBoard.Repositories
{
    public interface IBoardRepository
    {
        Task<IEnumerable<BoardVm>> GetBoardDetail(int empId, int projectId);
    }
}
