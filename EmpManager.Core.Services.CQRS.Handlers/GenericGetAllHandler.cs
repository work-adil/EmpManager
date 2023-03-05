using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmpManager.Core.Domain;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    public class GenericGetAllHandler<TModel, TRequest, TResponse, THandler> : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<List<TResponse>>, THandler>
        where TRequest : IRequest<GenericBaseResult<List<TResponse>>>
        where TModel : class
        where THandler : RepositoryHandlerBase<TModel, TRequest, GenericBaseResult<List<TResponse>>, THandler>
    {
        public GenericGetAllHandler(IGenericRepository<TModel> repository, IMapper mapper, ILogger<THandler> logger) : base(repository, mapper, logger)
        {
        }

        protected override Task<GenericBaseResult<List<TResponse>>> OnHandleRequest(TRequest request, CancellationToken cancellationToken)
            => GenerateResult(async () => await ModifyQuery(request, Repository.GetQueryable()).ProjectTo<TResponse>(MapperConfiguration).ToListAsync(cancellationToken));

        /// <summary>
        /// Modifies query based on request.
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="query">Query</param>
        /// <returns>Modified Query</returns>
        protected virtual IQueryable<TModel> ModifyQuery(TRequest request, IQueryable<TModel> query) => query;
    }
}
