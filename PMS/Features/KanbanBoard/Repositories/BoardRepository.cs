using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.KanbanBoard.ViewModels;

namespace PMS.Features.KanbanBoard.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly PmsDbContext _dbContext;

        public BoardRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BoardVm>> GetBoardDetail(int empId, int projectId)
        {
            var empModel = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == empId);

            var sprintModel = await _dbContext.Sprints.Where(x => x.IsDeleted == false).ToListAsync();

            var projectModels = await _dbContext.ProjectTasks
                        .Where(x => x.ProjectId == projectId && x.EmployeeId == empId && x.IsDeleted == false)
                            .Select(x => new BoardVm()
                            {
                                DueDate = x.DueDate,
                                ModuleName = x.ModuleName ?? string.Empty,
                                Priority = x.TaskPriority ?? string.Empty,
                                TaskCode = x.TaskCode ?? string.Empty,
                                TaskName = x.TaskName ?? string.Empty,
                                TaskType = x.TaskType ?? string.Empty,
                                StartDate = x.StartDate,
                                AssignedTo = $"{empModel.Name ?? ""} ({empModel.EmployeeCode ?? string.Empty})",
                                TaskStatus = x.TaskStatus ?? "N/A",
                                SprintName = string.Empty,
                                Id = x.Id,
                                Sequence = GetSequenceNumber(x.TaskStatus ?? string.Empty)

                            }).ToListAsync();

            return projectModels;
        }

        private static int GetSequenceNumber(string taskStatus)
        {
            return taskStatus switch
            {
                "In Progress" => 2,
                "Not Started" => 1,
                "Pending QA" => 3,
                "QA Approved" => 4,
                "Completed" => 5,
                _ => 99999
            };

        }
    }
}
