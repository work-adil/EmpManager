using AutoMapper;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Departments
{
    public class AddDepartmentHandler : GenericAddHandler<Department, AddDepartmentCommand, DepartmentResponse, AddDepartmentHandler>
    {
        public AddDepartmentHandler(Domain.IGenericRepository<Department> repository, IMapper mapper, ILogger<AddDepartmentHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
