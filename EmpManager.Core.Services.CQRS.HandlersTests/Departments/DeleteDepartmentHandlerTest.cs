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
    public class DeleteDepartmentHandlerTests
    {
        private readonly DeleteDepartmentHandler _updateDepartmentHandler;
        private readonly List<Department> departments = new List<Department>();
        private readonly string OriginalDepartment = "OriginalDepartment";
        public DeleteDepartmentHandlerTests()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Department>>();
            var departmentRepoMock = new Mock<IGenericRepository<Department>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<DeleteDepartmentHandler>>();

            repoMock.Setup(x => x.DeleteByIdAsync(It.IsAny<string>(), default)).Returns((string id, CancellationToken ct) =>
            {
                var oldEmp = departments.First(x=> x.Id == id);
                departments.Remove(oldEmp);
                return Task.CompletedTask;
            });

            mapperMock.Setup(x => x.Map<DepartmentResponse>(It.IsAny<Department>())).Returns((Department x)
                 => new DepartmentResponse { Id = x.Id, Name = x.Name});

            mapperMock.Setup(x => x.Map<Department>(It.IsAny<DepartmentResponse>())).Returns((DepartmentResponse x)
                => new Department { Id = x.Id, Name = x.Name});           

            _updateDepartmentHandler = new DeleteDepartmentHandler(repoMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task DeleteDepartmentHandler_Should_Handle_DeleteDepartmentCommand()
        {
            // Arrange.
            var command = new DeleteDepartmentCommand { Id = OriginalDepartment};

            // Act.
            await _updateDepartmentHandler.Handle(command, CancellationToken.None);
            
            // Assert.
            departments.Count.Should().Be(0);
        }

        private void SetupData()
        {
            departments.Add(new Department { Id = OriginalDepartment, Name = "Department1" });
        }
    }
}
