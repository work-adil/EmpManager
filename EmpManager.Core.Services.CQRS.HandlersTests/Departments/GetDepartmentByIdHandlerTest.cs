using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Handlers.Departments;
using EmpManager.Core.Services.CQRS.Queries.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmpManager.Core.Services.CQRS.HandlersTests.Departments
{

    [TestClass()]
    public class GetDepartmentByIdHandlerTest
    {
        private readonly GetDepartmentByIdHandler _getDepartmentByIdHandler;
        private readonly List<Department> departments = new List<Department>();
        private readonly string OriginalDepartment = "OriginalDepartment";

        public GetDepartmentByIdHandlerTest()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Department>>();
            var departmentRepoMock = new Mock<IGenericRepository<Department>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<GetDepartmentByIdHandler>>();

            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), default)).ReturnsAsync((string id, CancellationToken ct) =>
            {
                return departments.FirstOrDefault(x=> x.Id == id);
            });

            mapperMock.Setup(x => x.Map<DepartmentResponse>(It.IsAny<Department>())).Returns((Department x)
                 => new DepartmentResponse { Id = x.Id, Name = x.Name });

            mapperMock.Setup(x => x.Map<Department>(It.IsAny<DepartmentResponse>())).Returns((DepartmentResponse x)
                => new Department { Id = x.Id, Name = x.Name });

            _getDepartmentByIdHandler = new GetDepartmentByIdHandler(repoMock.Object, mapperMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task GetDepartmentByIdHandler_Should_Handle_GetDepartmentByIdQuery()
        {
            // Arrange.
            var query = new GetDepartmentByIdQuery { Id = OriginalDepartment };

            // Act.
            var result = (await _getDepartmentByIdHandler.Handle(query, CancellationToken.None))!.Result!;

            // Assert.
            var lastDepartment = departments.Last();
            result.Name.Should().Be(lastDepartment.Name);
            result.Id.Should().Be(lastDepartment.Id);
        }

        private void SetupData()
        {
            departments.Add(new Department { Id = OriginalDepartment, Name = "Department1" });
        }
    }
}
