using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Handlers.Employees;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmpManager.Core.Services.CQRS.HandlersTests.Employees
{

    [TestClass()]
    public class UpdateEmployeeHandlerTests
    {
        private readonly UpdateEmployeeHandler _updateEmployeeHandler;
        private readonly List<Employee> employees = new List<Employee>();
        private readonly string OriginalEmployee = "OriginalEmployee";
        public UpdateEmployeeHandlerTests()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Employee>>();
            var employeeRepoMock = new Mock<IGenericRepository<Employee>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UpdateEmployeeHandler>>();

            repoMock.Setup(x => x.UpdateAsync(It.IsAny<Employee>(), default)).ReturnsAsync((Employee emp, CancellationToken ct) =>
            {
                var oldEmp = employees.First(x=> x.Id == emp.Id);
                oldEmp.Name = emp.Name;
                oldEmp.Email = emp.Email;
                oldEmp.DepartmentId = emp.DepartmentId;
                return oldEmp;
            });

            mapperMock.Setup(x => x.Map<EmployeeResponse>(It.IsAny<Employee>())).Returns((Employee x)
                 => new EmployeeResponse { Id = x.Id, Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId});

            mapperMock.Setup(x => x.Map<Employee>(It.IsAny<EmployeeResponse>())).Returns((EmployeeResponse x)
                => new Employee { Id = x.Id, Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId });

            mapperMock.Setup(x => x.Map<UpdateEmployeeCommand>(It.IsAny<Employee>())).Returns((Employee x)
                => new UpdateEmployeeCommand { Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId, Id = x.Id });

            mapperMock.Setup(x => x.Map<Employee>(It.IsAny<UpdateEmployeeCommand>())).Returns((UpdateEmployeeCommand x)
                => new Employee { Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId, Id = x.Id });

            _updateEmployeeHandler = new UpdateEmployeeHandler(repoMock.Object, mapperMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task UpdateEmployeeHandler_Should_Handle_UpdateEmployeeCommand()
        {
            // Arrange.
            var command = new UpdateEmployeeCommand { Id = OriginalEmployee, Name = "Employee 11", Email = "a@b.com", DepartmentId = "Dep1"};

            // Act.
            var result = (await _updateEmployeeHandler.Handle(command, CancellationToken.None))!.Result!;
            
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
