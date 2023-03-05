using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Handlers;
using EmpManager.Core.Services.CQRS.Handlers.Departments;
using EmpManager.Core.Services.CQRS.Queries.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;

namespace EmpManager.Core.Services.CQRS.HandlersTests.Departments
{

    [TestClass()]
    public class GetAllDepartmentsHandlerTest
    {
        private readonly GetAllDepartmentsHandler _getAllDepartmentsHandler;
        private readonly List<Department> departments = new List<Department>();

        public GetAllDepartmentsHandlerTest()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Department>>();
            var departmentRepoMock = new Mock<IGenericRepository<Department>>();
            var mockAutoMapper = new MapperConfiguration(mc => mc.AddProfile(new UserProfile())).CreateMapper().ConfigurationProvider;
            var loggerMock = new Mock<ILogger<GetAllDepartmentsHandler>>();
            var queryableDepartments = departments.BuildMock();

            repoMock.Setup(x => x.GetQueryable(It.IsAny<bool>())).Returns((bool isNoTracking) => queryableDepartments.AsQueryable());

            _getAllDepartmentsHandler = new GetAllDepartmentsHandler(repoMock.Object, mockAutoMapper.CreateMapper(), loggerMock.Object);
        }

        [TestMethod()]
        public async Task GetAllDepartmentsHandler_Should_Handle_GetAllDepartmentsQuery()
        {
            // Arrange.
            var query = new GetAllDepartmentsQuery();

            // Act.
            var result = (await _getAllDepartmentsHandler.Handle(query, CancellationToken.None))!.Result!;

            // Assert.
            result.Count.Should().Be(2);
        }

        private void SetupData()
        {
            departments.Add(new Department { Id = "Emp1", Name = "Department1" });
            departments.Add(new Department { Id = "Emp2", Name = "Department2" });
        }
    }
}
