using AutoMapper;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    /// <summary>
    /// Base class for handlers.
    /// </summary>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    /// <typeparam name="TResponse">Type of response.</typeparam>
    /// <typeparam name="THandler">Type of handler.</typeparam>
    public abstract class HandlerBase<TRequest, TResponse, THandler> : IRequestHandler<TRequest, TResponse>, IMap
       where TRequest : IRequest<TResponse>
       where THandler : HandlerBase<TRequest, TResponse, THandler>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<THandler> _logger;
        protected HandlerBase(IMapper mapper, ILogger<THandler> logger)
        => (_mapper, MapperConfiguration, _logger) = (mapper, mapper.ConfigurationProvider, logger);

        /// <summary>
        /// Mapper configuration
        /// </summary>
        protected IConfigurationProvider MapperConfiguration { get; }

        /// <summary>
        /// Map funciton to map.
        /// </summary>
        /// <typeparam name="TDest">Destination type.</typeparam>
        /// <param name="source">Source type.</param>
        /// <returns>Desitnation object</returns>
        public TDest Map<TDest>(object? source) => _mapper.Map<TDest>(source);

        /// <summary>
        /// Handles 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling Request");
            var result = OnHandleRequest(request, cancellationToken);
            _logger.LogInformation("Finished Handling Request");
            return result;
        }

        /// <summary>
        /// Generate Result.
        /// </summary>
        /// <typeparam name="TResponse">Type of response model.</typeparam>
        /// <param name="createResponse">Function for creating response</param>
        /// <returns>Generated Result</returns>
        protected async Task<GenericBaseResult<TResponse>> GenerateResult<TResponse>(Func<Task<TResponse>> createResponse)
        {
            try
            {
                return new GenericBaseResult<TResponse>(await createResponse());
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                var result = new GenericBaseResult<TResponse>(default);
                result.AddExceptionLog(ex);
                return result;
            }
        }

        /// <summary>
        /// Generate Result
        /// </summary>
        /// <typeparam name="TResponse">Type of Response Model</typeparam>
        /// <param name="createResponse">Function Creating response</param>
        /// <returns>Generated Result</returns>
        protected GenericBaseResult<TResponse> GenerateResult(Func<TResponse> createResponse)
        {
            try
            {
                return new GenericBaseResult<TResponse>(createResponse());
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                var result = new GenericBaseResult<TResponse>(default);
                result.AddExceptionLog(ex);
                return result;
            }
        }

        /// <summary>
        /// Generate Base Result
        /// </summary>
        /// <typeparam name="TResponse">Type of Response Model</typeparam>
        /// <param name="createResponse">Function Creating response</param>
        /// <returns>Generated Result</returns>
        protected async Task<BaseResult> GenerateBaseResult(Func<Task> createResponse)
        {
            try
            {
                await createResponse();
                return new BaseResult();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                var result = new BaseResult();
                result.AddExceptionLog(ex);
                return result;
            }
        }

        /// <summary>
        /// Generate Base Result
        /// </summary>
        /// <param name="createResponse">Function Creating response</param>
        /// <returns>Generated Result</returns>
        protected BaseResult GenerateBaseResult(Action createResponse)
        {
            try
            {
                createResponse();
                return new BaseResult();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                var result = new BaseResult();
                result.AddExceptionLog(ex);
                return result;
            }
        }

        protected abstract Task<TResponse> OnHandleRequest(TRequest request, CancellationToken cancellationToken);
    }
}
