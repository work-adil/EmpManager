using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Employees
{
    public class DeleteEmployeeHandler : GenericDeleteHandler<Employee, DeleteEmployeeCommand, DeleteEmployeeHandler>
    {
        public DeleteEmployeeHandler(IGenericRepository<Employee> repository, ILogger<DeleteEmployeeHandler> logger) : base(repository, logger)
        {
        }
    }
}
