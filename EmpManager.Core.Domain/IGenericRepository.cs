namespace EmpManager.Core.Domain
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        /// <summary>
        /// Adds entity.
        /// </summary>
        /// <param name="model">Entity to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Added entity.</returns>
        Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes entity.
        /// </summary>
        /// <param name="model">Entity to remove.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task.</returns>
        Task DeleteByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get model by Id.
        /// </summary>
        /// <param name="id">Id of the entity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Entity if found otherwise null.</returns>
        ValueTask<TModel?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets queryable.
        /// </summary>
        /// <param name="isNoTracking">Is no tracking (true by default for best performance).</param>
        /// <returns>Queryable.</returns>
        IQueryable<TModel> GetQueryable(bool isNoTracking = true);

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="model">Entity to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Updated entity.</returns>
        Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default);
    }
}
