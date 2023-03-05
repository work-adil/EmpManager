using AutoMapper;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Employees
{
    public class AddEmployeeHandler : GenericAddHandler<Employee, AddEmployeeCommand, EmployeeResponse, AddEmployeeHandler>
    {
        public AddEmployeeHandler(Domain.IGenericRepository<Employee> repository, IMapper mapper, ILogger<AddEmployeeHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
