using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Services.CQRS.Queries;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    public abstract class GenericGetByIdHandler<TModel, TRequest, TResponse, THandler> : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<TResponse>, THandler>
        where TRequest : GuidModelBase, IRequest<GenericBaseResult<TResponse>>
        where TModel : class
        where THandler : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<TResponse>, THandler>
        where TResponse : class
    {
        protected GenericGetByIdHandler(IGenericRepository<TModel> repository, IMapper mapper, ILogger<THandler> logger) : base(repository, mapper, logger)
        {
        }

        protected override async Task<GenericBaseResult<TResponse>> OnHandleRequest(TRequest request, CancellationToken cancellationToken)
        {
            var result = await GenerateResult(async () => Map<TResponse>(await Repository.GetByIdAsync(request.Id, cancellationToken)));
            if (result != null && result.Result == null)
                result.ResponseStatusCode = System.Net.HttpStatusCode.NotFound;
            return result!;
        }
    }
}
