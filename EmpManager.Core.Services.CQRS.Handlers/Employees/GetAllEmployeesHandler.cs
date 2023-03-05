using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Employees
{
    public class GetAllEmployeesHandler : GenericGetAllHandler<Employee, GetAllEmployeesQuery, EmployeeResponse, GetAllEmployeesHandler>
    {
        public GetAllEmployeesHandler(IGenericRepository<Employee> repository, IMapper mapper, ILogger<GetAllEmployeesHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
