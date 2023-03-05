using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Queries.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmpManager.Core.Services.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : CrudApiBase<DepartmentResponse, DepartmentResponse, GetDepartmentByIdQuery, GetAllDepartmentsQuery, AddDepartmentCommand, DeleteDepartmentCommand, UpdateDepartmentCommand, DeleteDepartmentCommand>
    {
        public DepartmentsController(IMediator mediator) : base(mediator)
        {
        }
    }
}
