using EmpManager.Core.Services.CQRS.Responses;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Commands.Employees
{
    public class DeleteEmployeeCommand : GuidModelBase, IRequest
    {
    }
}
