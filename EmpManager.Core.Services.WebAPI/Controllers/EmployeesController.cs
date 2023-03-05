using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Queries.Employees;
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
    }
}
