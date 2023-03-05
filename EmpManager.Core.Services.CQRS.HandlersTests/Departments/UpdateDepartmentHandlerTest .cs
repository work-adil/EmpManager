using AutoMapper;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Handlers.Departments;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmpManager.Core.Services.CQRS.HandlersTests.Departments
{

    [TestClass()]
    public class UpdateDepartmentHandlerTests
    {
        private readonly UpdateDepartmentHandler _updateDepartmentHandler;
        private readonly List<Department> departments = new List<Department>();
        private readonly string OriginalDepartment = "OriginalDepartment";
        public UpdateDepartmentHandlerTests()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Department>>();
            var departmentRepoMock = new Mock<IGenericRepository<Department>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UpdateDepartmentHandler>>();

            repoMock.Setup(x => x.UpdateAsync(It.IsAny<Department>(), default)).ReturnsAsync((Department emp, CancellationToken ct) =>
            {
                var oldDep = departments.First(x=> x.Id == emp.Id);
                oldDep.Name = emp.Name;
                return oldDep;
            });

            mapperMock.Setup(x => x.Map<DepartmentResponse>(It.IsAny<Department>())).Returns((Department x)
                 => new DepartmentResponse { Id = x.Id, Name = x.Name });

            mapperMock.Setup(x => x.Map<Department>(It.IsAny<DepartmentResponse>())).Returns((DepartmentResponse x)
                => new Department { Id = x.Id, Name = x.Name });

            mapperMock.Setup(x => x.Map<UpdateDepartmentCommand>(It.IsAny<Department>())).Returns((Department x)
                => new UpdateDepartmentCommand { Id = x.Id, Name = x.Name });

            mapperMock.Setup(x => x.Map<Department>(It.IsAny<UpdateDepartmentCommand>())).Returns((UpdateDepartmentCommand x)
                => new Department { Id = x.Id, Name = x.Name });

            _updateDepartmentHandler = new UpdateDepartmentHandler(repoMock.Object, mapperMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task UpdateDepartmentHandler_Should_Handle_UpdateDepartmentCommand()
        {
            // Arrange.
            var command = new UpdateDepartmentCommand { Id = OriginalDepartment, Name = "Department 11" };

            // Act.
            var result = (await _updateDepartmentHandler.Handle(command, CancellationToken.None))!.Result!;
            
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
