using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Queries.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Departments
{
    public class GetAllDepartmentsHandler : GenericGetAllHandler<Department, GetAllDepartmentsQuery, DepartmentResponse, GetAllDepartmentsHandler>
    {
        public GetAllDepartmentsHandler(IGenericRepository<Department> repository, IMapper mapper, ILogger<GetAllDepartmentsHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
