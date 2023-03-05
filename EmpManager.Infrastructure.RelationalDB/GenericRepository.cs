using EmpManager.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmpManager.Infrastructure.RelationalDb
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// Gets model by Id.
        /// </summary>
        /// <param name="id">Id of the model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Entity if found otherwise null.</returns>
        public ValueTask<TModel?> GetByIdAsync(object id, CancellationToken cancellationToken = default) => _dbContext.Set<TModel>().FindAsync(new[] { id }, cancellationToken);


        /// <summary>
        /// Gets queryable.
        /// </summary>
        /// <param name="isNoTracking">Is no tracking (true by default for best performance).</param>
        /// <returns>Queryable.</returns>
        public IQueryable<TModel> GetQueryable(bool isNoTracking = true) => isNoTracking ? _dbContext.Set<TModel>().AsQueryable().AsNoTracking() : _dbContext.Set<TModel>().AsQueryable();


        /// <summary>
        /// Adds entity.
        /// </summary>
        /// <param name="model">Entity to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Added entity.</returns>
        public async Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        /// <summary>
        /// Removes entity.
        /// </summary>
        /// <param name="model">Entity to remove.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task.</returns>
        public async Task DeleteByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var entityToRemove = await _dbContext.Set<TModel>().FindAsync(new[] { id }, cancellationToken);
            if (entityToRemove != null)
            {
                _dbContext.Set<TModel>().Remove(entityToRemove);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="model">Entity to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Updated entity.</returns>
        public async Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task.</returns>
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
