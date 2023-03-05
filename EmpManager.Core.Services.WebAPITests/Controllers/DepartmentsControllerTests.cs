using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Queries.Departments;
using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using EmpManager.Core.Services.WebAPI.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmpManager.Core.Services.WebAPITests.Controllers
{
    [TestClass()]
    public class DepartmentsControllerTests
    {
        private readonly List<DepartmentResponse> departments = new List<DepartmentResponse>();
        private readonly DepartmentsController departmentsController;

        public DepartmentsControllerTests()
        {
            // Set up mock
            var mock = new Mock<IMediator>();
            mock.Setup(x => x.Send(It.IsAny<GetAllDepartmentsQuery>(), default(CancellationToken))).ReturnsAsync(new GenericBaseResult<List<DepartmentResponse>>(departments));
            mock.Setup(x => x.Send(It.IsAny<GetDepartmentByIdQuery>(), default(CancellationToken))).ReturnsAsync((GetDepartmentByIdQuery q, CancellationToken ct) =>
            {
                var Department = departments.FirstOrDefault(x => x.Id == q.Id);
                var result = new GenericBaseResult<DepartmentResponse>(Department);
                if (Department == null)
                    result.ResponseStatusCode = System.Net.HttpStatusCode.NotFound;
                return result;
            });

            mock.Setup(x => x.Send(It.IsAny<UpdateDepartmentCommand>(), default(CancellationToken))).ReturnsAsync((UpdateDepartmentCommand u, CancellationToken ct) =>
            {
                var departmentToUpdate = departments.First(x => x.Id == u.Id);
                departmentToUpdate.Name = u.Name;
                return new GenericBaseResult<DepartmentResponse>(departmentToUpdate);
                });
            mock.Setup(x => x.Send(It.IsAny<DeleteDepartmentCommand>(), default(CancellationToken))).Returns((DeleteDepartmentCommand d, CancellationToken ct) =>
            {
                departments.Remove(departments.First(x => x.Id == d.Id)!);
                return Task.CompletedTask;
            });

            mock.Setup(x => x.Send(It.IsAny<AddDepartmentCommand>(), default(CancellationToken))).ReturnsAsync((AddDepartmentCommand a, CancellationToken ct) =>
            {
                var departmentToAdd = new DepartmentResponse 
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = a.Name
                };
                departments.Add(departmentToAdd);
                return new GenericBaseResult<DepartmentResponse>(departmentToAdd);
            });

            // Setup Departments
            SetupDepartments();
            departmentsController = new DepartmentsController(mock.Object);
            departmentsController.ControllerContext = new ControllerContext();
        }

        private void SetupDepartments()
        {
            departments.Add(new DepartmentResponse { Id = "Dep1", Name = "Department 1"});
            departments.Add(new DepartmentResponse { Id = "Dep2", Name = "Department 2"});
            departments.Add(new DepartmentResponse { Id = "Dep3", Name = "Department 3"});

        }

        [DataTestMethod()]
        [DataRow("Dep1", 0)]
        [DataRow("Dep3", 2)]
        public async Task GetEntityByIdShould_Return_Department_With_Id(string entityId, int index)
        {
            // Arrange, Act.
            var department = (((await departmentsController.GetEntityById(entityId )).Result as OkObjectResult)!.Value as GenericBaseResult<DepartmentResponse>)!.Result;
            
            // Assert.
            department.Should().Be(departments[index]);
        }

        [TestMethod()]
        public async Task GetEntityByIdShould_Return_NotFound_If_InvalidId()
        {
            // Arrange.
            const string InvalidId = "AA";

            // Act.
            var departmentResponse = (await departmentsController.GetEntityById(InvalidId)).Result;
            
            // Assert.
            departmentResponse.Should().BeOfType<NotFoundResult>();

        }

        [TestMethod()]
        public async Task GetEntities_Should_Return_All_Entities()
        {
            // Arrange, Act.
            var departmentsLoaded = ((await departmentsController.GetEntities()).Result as OkObjectResult)!.Value as GenericBaseResult<List<DepartmentResponse>>;
            
            // Assert.
            departmentsLoaded!.Result.Should().BeSameAs(departments);
        }

        [TestMethod()]
        public async Task DeleteEntity_Should_Delete_department_With_Id()
        {
            // Arrange.
            var deleteCommand = new DeleteDepartmentCommand { Id = "Dep2" };
            
            // Act.
            await departmentsController.DeleteEntity(deleteCommand);
            
            // Assert.
            departments.FirstOrDefault(x => x.Id == deleteCommand.Id).Should().BeNull();
        }

        [TestMethod()]
        public async Task UpdateEntity_Should_Updatedepartment()
        {
            // Arrange.
            const string DepartmentId = "Dep2";
            var departmentToUpdate = new UpdateDepartmentCommand
            {
                Id = DepartmentId,
                Name = "Updated Name"
            };

            // Act.
            var department = (((await departmentsController.UpdateEntity(departmentToUpdate)).Result as OkObjectResult)!.Value as GenericBaseResult<DepartmentResponse>)!.Result;
            
            // Assert
            var expectedDepartment = departments.FirstOrDefault(x => x.Id == DepartmentId);
            department.Should().Be(expectedDepartment);
            expectedDepartment!.Name.Should().Be(departmentToUpdate.Name);
        }

        [TestMethod()]
        public async Task AddEntity_Should_Add_New_department()
        {
            // Arrange.
            var adddepartmentCommand = new AddDepartmentCommand
            {
                Name = "New Department"
            };

            // Act.
            await departmentsController.AddEntity(adddepartmentCommand);
            
            // Assert.
            var lastDepartment = departments.Last();
            lastDepartment.Name.Should().Be(adddepartmentCommand.Name);
        }
    }
}