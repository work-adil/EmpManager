using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpManager.Core.Services.CQRS.Handlers.Employees
{
    public class FindEmployeesHandler : RepositoryHandlerBase<Employee, FindEmployeesQuery, GenericBaseResult<List<EmployeeResponse>>, FindEmployeesHandler>
    {
        private readonly IGenericRepository<Department> _depRepository;
        public FindEmployeesHandler(IGenericRepository<Department> depRepository, IGenericRepository<Employee> repository, IMapper mapper, ILogger<FindEmployeesHandler> logger) : base(repository, mapper, logger)
        {
            _depRepository = depRepository;
        }

        protected override async Task<GenericBaseResult<List<EmployeeResponse>>> OnHandleRequest(FindEmployeesQuery request, CancellationToken cancellationToken)
        {
            var searchStr = request.SearchString.ToLower();
            var employees = await Repository.GetQueryable().Where(x=> string.IsNullOrWhiteSpace(searchStr) || x.Name.ToLower().Contains(searchStr) || x.Email.ToLower().Contains(searchStr) || x.Department.Name.ToLower().Contains(searchStr)).ProjectTo<EmployeeResponse>(MapperConfiguration).ToListAsync();

            return new GenericBaseResult<List<EmployeeResponse>>(employees);
        }
    }
}
