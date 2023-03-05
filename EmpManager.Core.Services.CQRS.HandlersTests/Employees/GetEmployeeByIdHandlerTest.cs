using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Handlers.Employees;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmpManager.Core.Services.CQRS.HandlersTests.Employees
{

    [TestClass()]
    public class GetEmployeeByIdHandlerTest
    {
        private readonly GetEmployeeByIdHandler _getEmployeeByIdHandler;
        private readonly List<Employee> employees = new List<Employee>();
        private readonly string OriginalEmployee = "OriginalEmployee";

        public GetEmployeeByIdHandlerTest()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Employee>>();
            var employeeRepoMock = new Mock<IGenericRepository<Employee>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<GetEmployeeByIdHandler>>();

            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), default)).ReturnsAsync((string id, CancellationToken ct) =>
            {
                return employees.FirstOrDefault(x=> x.Id == id);
            });

            mapperMock.Setup(x => x.Map<EmployeeResponse>(It.IsAny<Employee>())).Returns((Employee x)
                 => new EmployeeResponse { Id = x.Id, Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId});

            mapperMock.Setup(x => x.Map<Employee>(It.IsAny<EmployeeResponse>())).Returns((EmployeeResponse x)
                => new Employee { Id = x.Id, Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId });

            _getEmployeeByIdHandler = new GetEmployeeByIdHandler(repoMock.Object, mapperMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task GetEmployeeByIdHandler_Should_Handle_GetEmployeeByIdQuery()
        {
            // Arrange.
            var query = new GetEmployeeByIdQuery { Id = OriginalEmployee };

            // Act.
            var result = (await _getEmployeeByIdHandler.Handle(query, CancellationToken.None))!.Result!;

            // Assert.
            var lastEmployee = employees.Last();
            result.Name.Should().Be(lastEmployee.Name);
            result.Email.Should().Be(lastEmployee.Email);
            result.Id.Should().Be(lastEmployee.Id);
            result.DepartmentId.Should().Be(lastEmployee.DepartmentId);
        }

        private void SetupData()
        {
            employees.Add(new Employee { Id = OriginalEmployee, Name = "Employee1", DepartmentId = "Dep1", Email = "a@b.com" });
        }
    }
}
