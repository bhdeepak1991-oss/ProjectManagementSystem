
using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Sprint.Repositories
{
    public class SprintRepository : ISprintRepository
    {
        private readonly PmsDbContext _dbContext;

        public SprintRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> CreateSprint(Domains.Sprint model, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Sprints.AddAsync(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Sprint created successfully", true);
        }

        public async  Task<(string message, bool isSuccess, Domains.Sprint model)> GetSprintById(int id, CancellationToken cancellationToken)
        {
            var response= await _dbContext.Sprints.FindAsync(id);

            return ("Sprint fetched successfully", true, response ?? new());
        }

        public async Task<(string message, bool isSuccess, IEnumerable<Domains.Sprint> models)> GetSprintList(int projectId, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId==projectId).ToListAsync();

            return ("Sprint fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess)> UpdateSprint(Domains.Sprint model, CancellationToken cancellationToken)
        {
            var response = _dbContext.Sprints.Update(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Sprint updated Successfully !", true);
        }
    }
}
