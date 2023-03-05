using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmpManager.Core.Services.WebAPI.Controllers
{
    /// <summary>
    /// Base Class for APIs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBase : ControllerBase
    {
        public ApiBase(IMediator mediator)
            => (Mediator) = (mediator);

        /// <summary>
        /// Mediator to handle request.
        /// </summary>
        protected IMediator Mediator { get; }

        /// <summary>
        /// Gets response of the request.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        protected virtual async Task<TResult> GetResult<TResult>(IRequest<TResult> request)
            => await Mediator.Send(request);

        /// <summary>
        /// Gets response based on on the result.
        /// </summary>
        /// <typeparam name="TResult">Type of BaseResult.</typeparam>
        /// <param name="result">Result to convert to response.</param>
        /// <returns>Response.</returns>
        protected ActionResult<TResult> GetResponseFromResult<TResult>(TResult result) where TResult : BaseResult
        {
            return result.ResponseStatusCode switch
            {
                (HttpStatusCode.OK) => Ok(result),
                // To Get the Proble Details 
                (HttpStatusCode.NotFound) => NotFound(result),
                (HttpStatusCode.BadRequest) => BadRequest(result),
                _ => StatusCode((int)result.ResponseStatusCode, result)
            };
        }
    }
}
