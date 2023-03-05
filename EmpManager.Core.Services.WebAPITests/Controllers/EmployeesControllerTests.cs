using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Queries.Employees;
using EmpManager.Core.Services.CQRS.Responses;
using EmpManager.Core.Services.CQRS.Responses.Employees;
using EmpManager.Core.Services.WebAPI.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmpManager.Core.Services.WebAPITests.Controllers
{
    [TestClass()]
    public class EmployeesControllerTests
    {
        private readonly List<EmployeeResponse> employees = new List<EmployeeResponse>();
        private readonly EmployeesController employeesController;
        private static readonly string Dep1 = "Dep1";

        public EmployeesControllerTests()
        {
            // Set up mock
            var mock = new Mock<IMediator>();
            mock.Setup(x => x.Send(It.IsAny<GetAllEmployeesQuery>(), default(CancellationToken))).ReturnsAsync(new GenericBaseResult<List<EmployeeResponse>>(employees));
            mock.Setup(x => x.Send(It.IsAny<GetEmployeeByIdQuery>(), default(CancellationToken))).ReturnsAsync((GetEmployeeByIdQuery q, CancellationToken ct) =>
            {
                var Employee = employees.FirstOrDefault(x => x.Id == q.Id);
                var result = new GenericBaseResult<EmployeeResponse>(Employee);
                if (Employee == null)
                    result.ResponseStatusCode = System.Net.HttpStatusCode.NotFound;
                return result;
            });

            mock.Setup(x => x.Send(It.IsAny<UpdateEmployeeCommand>(), default(CancellationToken))).ReturnsAsync((UpdateEmployeeCommand u, CancellationToken ct) =>
            {
                var employeeToUpdate = employees.First(x => x.Id == u.Id);
                employeeToUpdate.Name = u.Name;
                employeeToUpdate.Phone = u.Phone;
                employeeToUpdate.DepartmentId = u.DepartmentId;
                return new GenericBaseResult<EmployeeResponse>(employeeToUpdate);
                });
            mock.Setup(x => x.Send(It.IsAny<DeleteEmployeeCommand>(), default(CancellationToken))).Returns((DeleteEmployeeCommand d, CancellationToken ct) =>
            {
                employees.Remove(employees.First(x => x.Id == d.Id));
                return Task.CompletedTask;
            });

            mock.Setup(x => x.Send(It.IsAny<AddEmployeeCommand>(), default(CancellationToken))).ReturnsAsync((AddEmployeeCommand a, CancellationToken ct) =>
            {
                var employeeToAdd = new EmployeeResponse 
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = a.Name,
                    Phone = a.Phone,
                    DepartmentId = a.DepartmentId
                };
                employees.Add(employeeToAdd);
                return new GenericBaseResult<EmployeeResponse>(employeeToAdd);
            });

            // Setup Employees
            SetupEmployees();
            employeesController = new EmployeesController(mock.Object);
            employeesController.ControllerContext = new ControllerContext();
        }

        private void SetupEmployees()
        {
            employees.Add(new EmployeeResponse { Id = "Emp1", Name = "Employee 1", DepartmentId = Dep1 , Phone = "001"});
            employees.Add(new EmployeeResponse { Id = "Emp2", Name = "Employee 2", DepartmentId = Dep1, Phone = "002" });
            employees.Add(new EmployeeResponse { Id = "Emp3", Name = "Employee 3", DepartmentId = Dep1, Phone = "003" });

        }

        [DataTestMethod()]
        [DataRow("Emp1", 0)]
        [DataRow("Emp3", 2)]
        public async Task GetEntityByIdShould_Return_Employee_With_Id(string entityId, int index)
        {
            // Arrange, Act.
            var employee = (((await employeesController.GetEntityById(entityId )).Result as OkObjectResult)!.Value as GenericBaseResult<EmployeeResponse>)!.Result;
            
            // Assert.
            employee.Should().Be(employees[index]);
        }

        [TestMethod()]
        public async Task GetEntityByIdShould_Return_NotFound_If_InvalidId()
        {
            // Arrange.
            const string InvalidId = "AA";
            
            // Act.
            var employeeResponse = (await employeesController.GetEntityById(InvalidId)).Result;
            
            // Assert.
            employeeResponse.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod()]
        public async Task GetEntities_Should_Return_All_Entities()
        {
            // Arrange, Act.
            var employeesLoaded = ((await employeesController.GetEntities()).Result as OkObjectResult)!.Value as GenericBaseResult<List<EmployeeResponse>>;
            
            // Assert.
            employeesLoaded!.Result.Should().BeSameAs(employees);
        }

        [TestMethod()]
        public async Task DeleteEntity_Should_Delete_employee_With_Id()
        {
            // Arrange.
            var deleteCommand = new DeleteEmployeeCommand { Id = "Emp2" };

            // Act.
            await employeesController.DeleteEntity(deleteCommand);

            // Assert.
            employees.FirstOrDefault(x => x.Id == deleteCommand.Id).Should().BeNull();
        }

        [TestMethod()]
        public async Task UpdateEntity_Should_Updateemployee()
        {
            // Arrange.
            const string EmployeeId = "Emp2";
            var employeeToUpdate = new UpdateEmployeeCommand
            {
                Id = EmployeeId,
                Name = "Updated Name",
                Phone = "007",
                DepartmentId = Dep1
            };

            // Act.
            var employee = (((await employeesController.UpdateEntity(employeeToUpdate)).Result as OkObjectResult)!.Value as GenericBaseResult<EmployeeResponse>)!.Result;
            
            // Assert.
            var expectedEmployee = employees.FirstOrDefault(x => x.Id == EmployeeId);
            employee.Should().Be(expectedEmployee);
            expectedEmployee!.Name.Should().Be(employeeToUpdate.Name);
            expectedEmployee.Phone.Should().Be(employeeToUpdate.Phone);
        }

        [TestMethod()]
        public async Task AddEntity_Should_Add_New_employee()
        {
            // Arrange.
            var addemployeeCommand = new AddEmployeeCommand
            {
                DepartmentId = Dep1,
                Name = "New Employee",
                Phone= "444"
            };

            // Act.
            await employeesController.AddEntity(addemployeeCommand);
            
            // Assert.
            var lastEmployee = employees.Last();
            lastEmployee.Name.Should().Be(addemployeeCommand.Name);
        }
    }
}