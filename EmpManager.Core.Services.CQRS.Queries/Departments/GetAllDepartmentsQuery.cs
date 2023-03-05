using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using MediatR;

namespace EmpManager.Core.Services.CQRS.Queries.Departments
{
    public class GetAllDepartmentsQuery : IRequest<GenericBaseResult<List<DepartmentResponse>>>
    {
    }
}
