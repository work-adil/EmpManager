using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Queries.Employees
{
    public class GetAllEmployeesQuery : IRequest<GenericBaseResult<List<EmployeeResponse>>>
    {
    }
}
