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
    public class DeleteEmployeeHandlerTests
    {
        private readonly DeleteEmployeeHandler _updateEmployeeHandler;
        private readonly List<Employee> employees = new List<Employee>();
        private readonly string OriginalEmployee = "OriginalEmployee";
        public DeleteEmployeeHandlerTests()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Employee>>();
            var employeeRepoMock = new Mock<IGenericRepository<Employee>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<DeleteEmployeeHandler>>();

            repoMock.Setup(x => x.DeleteByIdAsync(It.IsAny<string>(), default)).Returns((string id, CancellationToken ct) =>
            {
                var oldEmp = employees.First(x=> x.Id == id);
                employees.Remove(oldEmp);
                return Task.CompletedTask;
            });

            mapperMock.Setup(x => x.Map<EmployeeResponse>(It.IsAny<Employee>())).Returns((Employee x)
                 => new EmployeeResponse { Id = x.Id, Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId});

            mapperMock.Setup(x => x.Map<Employee>(It.IsAny<EmployeeResponse>())).Returns((EmployeeResponse x)
                => new Employee { Id = x.Id, Name = x.Name, Email = x.Email, DepartmentId = x.DepartmentId });           

            _updateEmployeeHandler = new DeleteEmployeeHandler(repoMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task DeleteEmployeeHandler_Should_Handle_DeleteEmployeeCommand()
        {
            // Arrange.
            var command = new DeleteEmployeeCommand { Id = OriginalEmployee};

            // Act.
            await _updateEmployeeHandler.Handle(command, CancellationToken.None);
            
            // Assert.
            employees.Count.Should().Be(0);
        }

        private void SetupData()
        {
            employees.Add(new Employee { Id = OriginalEmployee, Name = "Employee1", DepartmentId = "Dep1", Email = "a@b.com" });
        }
    }
}
