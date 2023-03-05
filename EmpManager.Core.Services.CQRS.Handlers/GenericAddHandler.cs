using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    /// <summary>
    /// Abstract generice base class for add commands.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    /// <typeparam name="TResponse">Type of resonse.</typeparam>
    /// <typeparam name="THandler">Type of handler.</typeparam>
    public abstract class GenericAddHandler<TModel, TRequest, TResponse, THandler> : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<TResponse>, THandler>
       where TRequest : IRequest<GenericBaseResult<TResponse>>
       where TModel : class
       where THandler : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<TResponse>, THandler>
       where TResponse : class
    {

        protected GenericAddHandler(IGenericRepository<TModel> repository, IMapper mapper, ILogger<THandler> logger) : base(repository, mapper, logger)
        {
        }

        protected override async Task<GenericBaseResult<TResponse>> OnHandleRequest(TRequest request, CancellationToken cancellationToken)
            => await GenerateResult(async () => Map<TResponse>(await Repository.AddAsync(Map<TModel>(request), cancellationToken)));
    }
}
