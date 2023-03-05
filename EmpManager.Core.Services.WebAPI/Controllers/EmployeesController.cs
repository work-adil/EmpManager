using Azure;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmpManager.Core.Services.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : CrudApiBase<EmployeeResponse, EmployeeResponse, GetEmployeeByIdQuery, GetAllEmployeesQuery, AddEmployeeCommand, DeleteEmployeeCommand, UpdateEmployeeCommand, DeleteEmployeeCommand>
    {
        public EmployeesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Searches employee by name, department or email.
        /// </summary>
        /// <param name="findEmployeesQuery">Search query.</param>
        /// <returns>Employees</returns>
        [HttpPost("Search")]
        public virtual async Task<ActionResult<GenericBaseResult<List<EmployeeResponse>>>> SearchEmployees(FindEmployeesQuery findEmployeesQuery)
            => GetResponseFromResult(await GetResult(findEmployeesQuery));
    }
}
