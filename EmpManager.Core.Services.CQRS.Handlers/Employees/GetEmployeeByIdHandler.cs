using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Employees
{
    public class GetEmployeeByIdHandler : GenericGetByIdHandler<Employee, GetEmployeeByIdQuery, EmployeeResponse, GetEmployeeByIdHandler>
    {
        public GetEmployeeByIdHandler(IGenericRepository<Employee> repository, IMapper mapper, ILogger<GetEmployeeByIdHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
