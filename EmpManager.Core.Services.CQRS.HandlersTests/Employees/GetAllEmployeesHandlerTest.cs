using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Handlers;
using EmpManager.Core.Services.CQRS.Handlers.Employees;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;

namespace EmpManager.Core.Services.CQRS.HandlersTests.Employees
{

    [TestClass()]
    public class GetAllEmployeesHandlerTest
    {
        private readonly GetAllEmployeesHandler _getAllEmployeesHandler;
        private readonly List<Employee> employees = new List<Employee>();

        public GetAllEmployeesHandlerTest()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Employee>>();
            var employeeRepoMock = new Mock<IGenericRepository<Employee>>();
            var mockAutoMapper = new MapperConfiguration(mc => mc.AddProfile(new UserProfile())).CreateMapper().ConfigurationProvider;
            var loggerMock = new Mock<ILogger<GetAllEmployeesHandler>>();
            var queryableEmployees = employees.BuildMock();
            repoMock.Setup(x => x.GetQueryable(It.IsAny<bool>())).Returns((bool isNoTracking) => queryableEmployees.AsQueryable());

            _getAllEmployeesHandler = new GetAllEmployeesHandler(repoMock.Object, mockAutoMapper.CreateMapper(), loggerMock.Object);
        }

        [TestMethod()]
        public async Task GetAllEmployeesHandler_Should_Handle_GetAllEmployeesQuery()
        {
            // Arrange.
            var query = new GetAllEmployeesQuery();

            // Act.
            var result = (await _getAllEmployeesHandler.Handle(query, CancellationToken.None))!.Result!;

            // Assert.
            result.Count.Should().Be(2);
        }

        private void SetupData()
        {
            employees.Add(new Employee { Id = "Emp1", Name = "Employee1", DepartmentId = "Dep1", Email = "007" });
            employees.Add(new Employee { Id = "Emp2", Name = "Employee2", DepartmentId = "Dep1", Email = "009" });
        }
    }
}
