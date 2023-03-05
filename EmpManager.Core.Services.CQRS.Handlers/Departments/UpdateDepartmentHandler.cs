using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Departments
{
    public class UpdateDepartmentHandler : GenericUpdateHandler<Department, UpdateDepartmentCommand, DepartmentResponse, UpdateDepartmentHandler>
    {
        public UpdateDepartmentHandler(IGenericRepository<Department> repository, IMapper mapper, ILogger<UpdateDepartmentHandler> logger) : base(repository, mapper, logger)
        {
        }
    }
}
