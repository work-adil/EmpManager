using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Queries.Employees
{
    public class FindEmployeesQuery : IRequest<GenericBaseResult<List<EmployeeResponse>>>
    {
        /// <summary>
        /// Search string to search email, name or department.
        /// </summary>
        public required string SearchString { get; set; }
    }
}
