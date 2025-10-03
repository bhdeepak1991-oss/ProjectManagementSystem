using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Master.Respositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PmsDbContext _context;

        public DepartmentRepository(PmsDbContext context)
        {
            _context = context;
        }
        public async Task<(string message, bool isSuccess)> CreateDepartment(DepartmentMaster model, CancellationToken cancellationToken)
        {
            var responseModel = await _context.DepartmentMasters.AddAsync(model, cancellationToken);

            var isExists = await _context.DepartmentMasters.AnyAsync(x => x.Name == model.Name && x.IsActive == true && x.IsDeleted == false, cancellationToken);

            if (isExists)
            {
                return ($"Department with name {model.Name} already present", false);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ("Department created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteDepartment(int id, CancellationToken cancellationToken)
        {
            var deleteModel = await _context.DepartmentMasters.FindAsync(id, cancellationToken);

            if (deleteModel is not null)
            {
                deleteModel.IsDeleted = true;

                deleteModel.IsActive = false;

                deleteModel.UpdatedDate = DateTime.Now;

                deleteModel.UpdatedBy = 1;

                _context.DepartmentMasters.Update(deleteModel);

                await _context.SaveChangesAsync(cancellationToken);

                return ("Department deleted successfully", true);
            }

            return ($"Record Not found for the Id {id}", false);

        }

        public async Task<(string message, bool isSuccess, IEnumerable<DepartmentMaster> model)> GetAllDepartments(CancellationToken cancellationToken)
        {
            var responseModels = await _context.DepartmentMasters.AsNoTracking()
                            .Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync(cancellationToken);

            return ("Departments fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess, DepartmentMaster? model)> GetDepartmentById(int id, CancellationToken cancellationToken)
        {
            var responseModel = await _context.DepartmentMasters.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id
                                && x.IsActive == true && x.IsDeleted == false, cancellationToken);

            return ("Department fetched successfully", true, responseModel);

        }

        public async Task<(string message, bool isSuccess)> UpdateDepartment(DepartmentMaster model, CancellationToken cancellationToken)
        {
            var responseModel = await _context.DepartmentMasters
                        .Where(x => x.Id != model.Id && x.IsDeleted == false)
                            .ToListAsync(cancellationToken);

            var isExists = responseModel.Any(x => x.Name == model.Name && x.IsActive == true);

            if (isExists)
            {
                return ($"Department with name {model.Name} already present mapped with another Id", false);
            }

            var updateModel = await _context.DepartmentMasters.FindAsync(model.Id, cancellationToken);

            if (updateModel is null)
            {
                return ($"Record Not found for the Id {model.Id}", false);
            }

            updateModel.Name = model.Name;

            updateModel.Description = model.Description;

            updateModel.IsActive = true;

            updateModel.UpdatedDate = DateTime.Now;

            _context.DepartmentMasters.Update(updateModel);

            await _context.SaveChangesAsync(cancellationToken);

            return ("Department updated successfully", true);
        }
    }
}
