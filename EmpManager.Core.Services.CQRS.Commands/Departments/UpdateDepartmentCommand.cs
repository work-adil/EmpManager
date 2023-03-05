using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Commands.Departments
{
    public class UpdateDepartmentCommand : GuidModelBase, IRequest<GenericBaseResult<DepartmentResponse>>
    {
        /// <summary>
        /// Name of the Department.
        /// </summary>
        public required string Name { get; set; }
    }
}
