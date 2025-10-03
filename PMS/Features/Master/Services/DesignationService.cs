using PMS.Domains;
using PMS.Features.Master.Respositories;
using PMS.Features.Master.ViewModels;

namespace PMS.Features.Master.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public DesignationService(IDesignationRepository designationRepository, IDepartmentRepository departmentRepository)
        {
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateDesignation(DesignationMaster model, CancellationToken cancellationToken)
        {
            return await _designationRepository.CreateDesignation(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteDesignation(int id, CancellationToken cancellationToken)
        {
            return await _designationRepository.DeleteDesignation(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<DesignationViewModel> model)> GetAllDesignation(CancellationToken cancellationToken)
        {
            var designationModels = await _designationRepository.GetAllDesignation(cancellationToken);
            var departmentModels = await _departmentRepository.GetAllDepartments(cancellationToken);

            var response = designationModels.model.ToList().Select(x => new DesignationViewModel()
            {
                Id = x.Id,
                Name = x?.Name ?? string.Empty,
                Description = x?.Description ?? string.Empty,
                DepartmentName = departmentModels.model?.FirstOrDefault(z => z.Id == x?.DepartmentId)?.Name ?? string.Empty,
                IsActive=x?.IsActive ?? false,
            }).ToList();

            return ("Data fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess, DesignationMaster? model)> GetDesignationById(int id, CancellationToken cancellationToken)
        {
            return await _designationRepository.GetDesignationById(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateDesignation(DesignationMaster model, CancellationToken cancellationToken)
        {
            return await _designationRepository.UpdateDesignation(model, cancellationToken);
        }
    }
}
