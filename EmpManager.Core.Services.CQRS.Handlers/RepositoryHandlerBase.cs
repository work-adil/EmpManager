using AutoMapper;
using EmpManager.Core.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    /// <summary>
    /// Base Handler for handling request with generic repository.
    /// </summary>
    /// <typeparam name="TModel">Data model type.</typeparam>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <typeparam name="THandler">Handler type.</typeparam>
    public abstract class RepositoryHandlerBase<TModel, TRequest, TResponse, THandler> : HandlerBase<TRequest, TResponse, THandler>

        where TRequest : IRequest<TResponse>
        where TModel : class
        where THandler : RepositoryHandlerBase<TModel, TRequest, TResponse, THandler>
    {
        protected RepositoryHandlerBase(IGenericRepository<TModel> repository, IMapper mapper, ILogger<THandler> logger) : base(mapper, logger)
          => (Repository) = (repository);

        /// <summary>
        /// Repository.
        /// </summary>
        protected IGenericRepository<TModel> Repository { get; }
    }
}