using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Queries.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Departments
{
    public class GetDepartmentByIdHandler : GenericGetByIdHandler<Department, GetDepartmentByIdQuery, DepartmentResponse, GetDepartmentByIdHandler>
    {
        public GetDepartmentByIdHandler(IGenericRepository<Department> repository, IMapper mapper, ILogger<GetDepartmentByIdHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
