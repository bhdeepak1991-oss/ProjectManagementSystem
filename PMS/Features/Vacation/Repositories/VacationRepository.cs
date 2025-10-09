using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.Vacation.ViewModels;

namespace PMS.Features.Vacation.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        private readonly PmsDbContext _dbContext;
        public VacationRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> CreateVacation(VacationDetail model, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.VacationDetails.AddAsync(model, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Vacation created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteVacationById(int id, CancellationToken cancellationToken)
        {
            var deleteModel = await _dbContext.VacationDetails.FindAsync(id, cancellationToken);

            if (deleteModel is null)
            {
                return ($"Vacation with Id {id} not found ", false);
            }

            deleteModel.IsDeleted = true;

            deleteModel.UpdatedDate = DateTime.Now;

            _dbContext.Update(deleteModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Vacation deleted successfully", true);

        }

        public async Task<(string message, bool isSuccess, IEnumerable<VacationVm> models)> GetVacationDetail(CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.VacationDetails.Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

            var employeeModels = await _dbContext.Employees.Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

            var response = responseModels.Select(x => new VacationVm
            {
                Id = x.Id,
                VacationName = x.VacationType ?? string.Empty,
                VacationType = x.VacationType ?? string.Empty,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                VacationDetailText = x.Reason,
                ProjectInvolved = x.ProjectInvolved == "0" ? "All" : x.ProjectInvolved,
                CreatedByName = employeeModels.FirstOrDefault(emp => emp.Id == x.Id)?.Name ?? string.Empty
            }).ToList();

            return ("Vacation fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess, VacationDetail model)> GetVacationModel(int id, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.VacationDetails.FindAsync(id, cancellationToken);

            if (responseModel is null)
            {
                return ($"vacation not found for Id {id}", false, new());
            }

            return ("Vacation feteched successfully", true, responseModel);
        }


        public async  Task<(string message, bool isSuccess)> UpdateVacation(VacationDetail model, CancellationToken cancellationToken)
        {
            _dbContext.VacationDetails.Update(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Vacation updated successfully", true);
        }
    }
}
