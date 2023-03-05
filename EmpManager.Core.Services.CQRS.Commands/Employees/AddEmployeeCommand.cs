using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Commands.Employees
{
    public class AddEmployeeCommand : IRequest<GenericBaseResult<EmployeeResponse>>
    {
        /// <summary>
        /// Name of the employee.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Id of the Department.
        /// </summary>
        public required string DepartmentId { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public required string Phone { get; set; }
    }
}
