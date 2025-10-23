using PMS.Features.KanbanBoard.Repositories;
using PMS.Features.KanbanBoard.ViewModels;

namespace PMS.Features.KanbanBoard.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async  Task<IEnumerable<BoardVm>> GetBoardDetail(int empId, int projectId)
        {
            return await _boardRepository.GetBoardDetail(empId, projectId);
        }
    }
}
