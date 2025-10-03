using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Master.Respositories
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly PmsDbContext _context;

        public DesignationRepository(PmsDbContext context)
        {
            _context = context;
        }
        public async Task<(string message, bool isSuccess)> CreateDesignation(DesignationMaster model, CancellationToken cancellationToken)
        {
            var responseModel = await _context.DesignationMasters.AddAsync(model, cancellationToken);

            var isExists = await _context.DepartmentMasters.AnyAsync(x => x.Name == model.Name && x.IsActive == true && x.IsDeleted == false, cancellationToken);

            if (isExists)
            {
                return ($"Designation with name {model.Name} already present", false);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ("Designation created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteDesignation(int id, CancellationToken cancellationToken)
        {
            var deleteModel = await _context.DesignationMasters.FindAsync(id, cancellationToken);

            if (deleteModel is not null)
            {
                deleteModel.IsDeleted = true;

                deleteModel.IsActive = false;

                deleteModel.UpdatedDate = DateTime.Now;

                deleteModel.UpdatedBy = 1;

                _context.DesignationMasters.Update(deleteModel);

                await _context.SaveChangesAsync(cancellationToken);

                return ("Designation deleted successfully", true);
            }

            return ($"Record Not found for the Id {id}", false);

        }

        public async Task<(string message, bool isSuccess, IEnumerable<DesignationMaster> model)> GetAllDesignation(CancellationToken cancellationToken)
        {
            var responseModels = await _context.DesignationMasters.AsNoTracking()
                            .Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync(cancellationToken);

            return ("Departments fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess, DesignationMaster? model)> GetDesignationById(int id, CancellationToken cancellationToken)
        {
            var responseModel = await _context.DesignationMasters.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id
                                && x.IsActive == true && x.IsDeleted == false, cancellationToken);

            return ("Designation fetched successfully", true, responseModel);

        }

        public async Task<(string message, bool isSuccess)> UpdateDesignation(DesignationMaster model, CancellationToken cancellationToken)
        {
            var responseModel = await _context.DesignationMasters
                        .Where(x => x.Id != model.Id && x.IsDeleted == false)
                            .ToListAsync(cancellationToken);

            var isExists = responseModel.Any(x => x.Name == model.Name && x.IsActive == true);

            if (isExists)
            {
                return ($"Designation with name {model.Name} already present mapped with another Id", false);
            }

            var updateModel = await _context.DesignationMasters.FindAsync(model.Id, cancellationToken);

            if (updateModel is null)
            {
                return ($"Record Not found for the Id {model.Id}", false);
            }

            updateModel.Name = model.Name;

            updateModel.Description = model.Description;

            updateModel.IsActive = true;

            updateModel.UpdatedDate = DateTime.Now;

            _context.DesignationMasters.Update(updateModel);

            await _context.SaveChangesAsync(cancellationToken);

            return ("Department updated successfully", true);
        }
    }
}
