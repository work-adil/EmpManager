using EmpManager.Core.Services.CQRS.Queries;
using EmpManager.Core.Services.CQRS.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmpManager.Core.Services.WebAPI.Controllers
{
    /// <summary>
    /// Abstract base class for CRUD API's Read Operations.
    /// </summary>
    /// <typeparam name="TResponse">>Type of response.</typeparam>
    /// <typeparam name="TByIdResponse">Type of GetById repsonse.</typeparam>
    /// <typeparam name="TGetByIdQuery">Type of GetById query.</typeparam>
    /// <typeparam name="TGetAllQuery">Type of GetAllQuery.</typeparam>
    public abstract class CrudApiBase<TResponse, TByIdResponse, TGetByIdQuery, TGetAllQuery, TAddCommand, TDeleteCommand, TUpdateCommand, TDeleteById> : ApiBase
        where TResponse : class
        where TByIdResponse : class
        where TGetByIdQuery : GuidModelBase, IRequest<GenericBaseResult<TByIdResponse>>, new()
        where TGetAllQuery : IRequest<GenericBaseResult<List<TResponse>>>, new()
        where TAddCommand : IRequest<GenericBaseResult<TResponse>>
        where TDeleteCommand : GuidModelBase, IRequest, new()
        where TUpdateCommand : IRequest<GenericBaseResult<TResponse>>
        where TDeleteById : class
    {

        protected CrudApiBase(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get an entity by Id.
        /// </summary>
        /// <param name="id">Id of the entity.</param>
        /// <returns>Entity if found otherwise NotFound.</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<GenericBaseResult<TByIdResponse>>> GetEntityById(string id)
        {
            var response = await GetEntityByIdResult(id);
            return response?.Result == null ? NotFound() : GetResponseFromResult(response);
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Id of the Entity.</param>
        /// <returns>Entity if found otherwise null.</returns>
        protected async Task<GenericBaseResult<TByIdResponse>> GetEntityByIdResult(string id)
        {
            var request = new TGetByIdQuery { Id = id };
            return await GetResult(request);
        }

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ActionResult<GenericBaseResult<List<TResponse>>>> GetEntities()
        {
            var request = new TGetAllQuery();            
            var response = await GetResult(request);
            return GetResponseFromResult(response);
        }

        /// <summary>
        /// Deletes an Entity by Id
        /// </summary>
        /// <param name="id">Id of the Entity to delete</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public virtual async Task<IActionResult> DeleteEntity(TDeleteById id)
        {
            var request = new TDeleteCommand();
            if (id is string idString)
                request.Id = idString;
            else if (id is TDeleteCommand deleteCommand)
                request = deleteCommand;
            await Mediator.Send(request);
            return Ok();
        }

        /// <summary>
        /// Updates an Entity
        /// </summary>
        /// <param name="updateEntityCommand">Entity to Update</param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<ActionResult<GenericBaseResult<TResponse>>> UpdateEntity(TUpdateCommand updateEntityCommand)
        {
            var entity = await GetResult(updateEntityCommand);
            return GetResponseFromResult(entity);
        }

        /// <summary>
        /// Adds an Entity
        /// </summary>
        /// <param name="addEntityCommand">Entity to Add</param>
        /// <returns>Entity Added Url</returns>
        [HttpPost]
        public virtual async Task<ActionResult<GenericBaseResult<TResponse>>> AddEntity(TAddCommand addEntityCommand)
        {
            var entity = await GetResult(addEntityCommand);
            return GetResponseFromResult(entity);
        }
    }
}
