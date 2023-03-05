using EmpManager.Core.Domain;
using EmpManager.Core.Services.CQRS.Queries;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    public class GenericDeleteHandler<TModel, TRequest, THandler> : IRequestHandler<TRequest>
         where TRequest : GuidModelBase, IRequest
         where TModel : class
         where THandler : GenericDeleteHandler<TModel, TRequest, THandler>
    {
        private readonly IGenericRepository<TModel> _repository;
        private readonly ILogger<THandler> _logger;
        public GenericDeleteHandler(IGenericRepository<TModel> repository, ILogger<THandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling Delete Command");
            await OnHandle(request, cancellationToken);
            _logger.LogInformation("Finished Handling Delete Command");
        }

        protected virtual Task OnHandle(TRequest request, CancellationToken cancellationToken)
            => _repository.DeleteByIdAsync(request.Id, cancellationToken);
    }
}
