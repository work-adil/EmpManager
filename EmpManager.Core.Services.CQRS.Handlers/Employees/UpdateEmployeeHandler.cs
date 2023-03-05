using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Employees
{
    public class UpdateEmployeeHandler : GenericUpdateHandler<Employee, UpdateEmployeeCommand, EmployeeResponse, UpdateEmployeeHandler>
    {
        public UpdateEmployeeHandler(IGenericRepository<Employee> repository, IMapper mapper, ILogger<UpdateEmployeeHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
