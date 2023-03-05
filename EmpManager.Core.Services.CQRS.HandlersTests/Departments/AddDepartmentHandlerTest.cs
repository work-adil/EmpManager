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
    public class AddDepartmentHandlerTests
    {
        private readonly AddDepartmentHandler _addDepartmentHandler;
        private readonly List<Department> departments = new List<Department>();

        public AddDepartmentHandlerTests()
        {
            var repoMock = new Mock<IGenericRepository<Department>>();
            var departmentRepoMock = new Mock<IGenericRepository<Department>>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<AddDepartmentHandler>>();

            repoMock.Setup(x => x.AddAsync(It.IsAny<Department>(), default)).ReturnsAsync((Department p, CancellationToken ct) =>
            {
                departments.Add(p);
                return p;
            });

            mapperMock.Setup(x => x.Map<DepartmentResponse>(It.IsAny<Department>())).Returns((Department x)
                 => new DepartmentResponse { Id = x.Id, Name = x.Name});

            mapperMock.Setup(x => x.Map<Department>(It.IsAny<DepartmentResponse>())).Returns((DepartmentResponse x)
                => new Department { Id = x.Id, Name = x.Name });

            mapperMock.Setup(x => x.Map<AddDepartmentCommand>(It.IsAny<Department>())).Returns((Department x)
                => new AddDepartmentCommand { Name = x.Name});

            mapperMock.Setup(x => x.Map<Department>(It.IsAny<AddDepartmentCommand>())).Returns((AddDepartmentCommand x)
                => new Department { Name = x.Name});

            _addDepartmentHandler = new AddDepartmentHandler(repoMock.Object, mapperMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task AddDepartmentHandler_Should_Handle_AddDepartmentCommand()
        {
            // Arrange.
            var command = new AddDepartmentCommand { Name = "New Department"};


            // Act.
            var result = (await _addDepartmentHandler.Handle(command, CancellationToken.None))!.Result!;

            // Assert.
            var lastDepartment = departments.Last();
            result.Name.Should().Be(lastDepartment.Name);
            result.Id.Should().Be(lastDepartment.Id);
        }
    }
}
