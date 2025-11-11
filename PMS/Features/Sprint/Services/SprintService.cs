using PMS.Features.Master.Respositories;
using PMS.Features.Sprint.Repositories;
using PMS.Features.Sprint.ViewModels;

namespace PMS.Features.Sprint.Services
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public SprintService(ISprintRepository sprintRepository, IDepartmentRepository departmentRepository)
        {
            _sprintRepository = sprintRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateSprint(Domains.Sprint model, CancellationToken cancellationToken)
        {
            return await _sprintRepository.CreateSprint(model, cancellationToken);
        }

        public async Task<string> GetSprintBusinessGoal(int sprintId, CancellationToken cancellationToken)
        {
            var responseModel = await _sprintRepository.GetSprintById(sprintId, cancellationToken);

            return responseModel.model.BusinessGoal ?? string.Empty;
        }

        public async Task<(string message, bool isSuccess, Domains.Sprint model)> GetSprintById(int id, CancellationToken cancellationToken)
        {
            return await _sprintRepository.GetSprintById(id, cancellationToken);
        }

        public async Task<string> GetSprintGoal(int sprintId, CancellationToken cancellationToken)
        {
            var responseModel = await _sprintRepository.GetSprintById(sprintId, cancellationToken);

            return responseModel.model.SprintGoal ?? string.Empty;
        }

        public async Task<(string message, bool isSuccess, IEnumerable<SprintViewModel> models)> GetSprintList(int projectId,CancellationToken cancellationToken)
        {
            var departmentResponse = await _departmentRepository.GetAllDepartments(cancellationToken);

            var sprintResponse = await _sprintRepository.GetSprintList(projectId,cancellationToken);

            var response = sprintResponse.models.ToList().Select(x => new SprintViewModel()
            {
                Id= x.Id,
                SprintName= x.SprintName,
                SprintGoal= x.SprintGoal,
                StartDate= x.StartDate,
                EndDate= x.EndDate,
                DurationDays= x.DurationDays,
                DemoDate= x.DemoDate,
                BusinessGoal= x.BusinessGoal,
                DepartmentName= departmentResponse.model.FirstOrDefault(z=>z.Id== x.DepartmentId)?.Name ?? string.Empty

            }).ToList();

            return ("Sprint fetched successfully", true, response);

        }

        public async Task<(string message, bool isSuccess)> UpdateSprint(Domains.Sprint model, CancellationToken cancellationToken)
        {
            return await _sprintRepository.UpdateSprint(model, cancellationToken);
        }
    }
}
