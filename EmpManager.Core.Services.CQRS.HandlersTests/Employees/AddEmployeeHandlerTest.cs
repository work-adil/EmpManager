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
    public class AddEmployeeHandlerTests
    {
        private readonly AddEmployeeHandler _addEmployeeHandler;
        private readonly List<Employee> employees = new List<Employee>();

        public AddEmployeeHandlerTests()
        {
            var repoMock = new Mock<IGenericRepository<Employee>>();
            var employeeRepoMock = new Mock<IGenericRepository<Employee>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<AddEmployeeHandler>>();

            repoMock.Setup(x => x.AddAsync(It.IsAny<Employee>(), default)).ReturnsAsync((Employee p, CancellationToken ct) =>
            {
                employees.Add(p);
                return p;
            });

            mapperMock.Setup(x => x.Map<EmployeeResponse>(It.IsAny<Employee>())).Returns((Employee x)
                 => new EmployeeResponse { Id = x.Id, Name = x.Name, Phone = x.Phone, DepartmentId = x.DepartmentId});

            mapperMock.Setup(x => x.Map<Employee>(It.IsAny<EmployeeResponse>())).Returns((EmployeeResponse x)
                => new Employee { Id = x.Id, Name = x.Name, Phone = x.Phone, DepartmentId = x.DepartmentId });

            mapperMock.Setup(x => x.Map<AddEmployeeCommand>(It.IsAny<Employee>())).Returns((Employee x)
                => new AddEmployeeCommand { Name = x.Name, Phone = x.Phone, DepartmentId = x.DepartmentId });

            mapperMock.Setup(x => x.Map<Employee>(It.IsAny<AddEmployeeCommand>())).Returns((AddEmployeeCommand x)
                => new Employee { Name = x.Name, Phone = x.Phone, DepartmentId = x.DepartmentId });

            _addEmployeeHandler = new AddEmployeeHandler(repoMock.Object, mapperMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task AddEmployeeHandler_Should_Handle_AddEmployeeCommand()
        {
            // Arrange.
            var command = new AddEmployeeCommand { Name = "New Employee", Phone = "007", DepartmentId = "Dep1"};


            // Act.
            var result = (await _addEmployeeHandler.Handle(command, CancellationToken.None))!.Result!;

            // Assert.
            var lastEmployee = employees.Last();
            result.Name.Should().Be(lastEmployee.Name);
            result.Phone.Should().Be(lastEmployee.Phone);
            result.Id.Should().Be(lastEmployee.Id);
            result.DepartmentId.Should().Be(lastEmployee.DepartmentId);
        }
    }
}
