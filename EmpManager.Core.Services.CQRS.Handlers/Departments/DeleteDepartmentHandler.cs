using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Departments
{
    public class DeleteDepartmentHandler : GenericDeleteHandler<Department, DeleteDepartmentCommand, DeleteDepartmentHandler>
    {
        public DeleteDepartmentHandler(IGenericRepository<Department> repository, ILogger<DeleteDepartmentHandler> logger) : base(repository, logger)
        {
        }
    }
}
