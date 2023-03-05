using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    public class GenericUpdateHandler<TModel, TRequest, TResponse, THandler> : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<TResponse>, THandler>
       where TRequest : IRequest<GenericBaseResult<TResponse>>
       where TModel : class
       where THandler : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<TResponse>, THandler>
       where TResponse : class
    {
        public GenericUpdateHandler(IGenericRepository<TModel> repository, IMapper mapper, ILogger<THandler> logger) : base(repository, mapper, logger)
        {
        }

        protected override async Task<GenericBaseResult<TResponse>> OnHandleRequest(TRequest request, CancellationToken cancellationToken)
            => await GenerateResult(async () => (Map<TResponse>(await Repository.UpdateAsync(Map<TModel>(request), cancellationToken))));
    }
}
