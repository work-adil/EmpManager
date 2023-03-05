using EmpManager.Core.Services.CQRS.Responses;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Commands.Departments
{
    public class DeleteDepartmentCommand : GuidModelBase, IRequest
    {
    }
}
