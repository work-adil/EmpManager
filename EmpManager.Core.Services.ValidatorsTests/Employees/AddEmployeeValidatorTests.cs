using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using FluentAssertions;

namespace EmpManager.Core.Services.Validators.Employees.Tests
{
    [TestClass()]
    public class AddEmployeeValidatorTests
    {
        private readonly AddEmployeeValidator _addEmployeeValidator;
        private readonly List<Department> _departmentList = new List<Department>();
        public AddEmployeeValidatorTests()
        {
            SetupData();
            var repoMock = new Mock<IGenericRepository<Department>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), default)).ReturnsAsync((string id, CancellationToken ct) => _departmentList.FirstOrDefault(x => x.Id == id));
            _addEmployeeValidator = new AddEmployeeValidator(repoMock.Object);
        }

        private void SetupData()
        {
            _departmentList.AddRange(new[]
            {
                new Department { Id = "ITDep", Name = "IT Department"},
                new Department { Id = "OPDep", Name = "Operation Department"}
            });
        }

        [TestMethod()]
        public void AddEmployeeValidatorTests_Should_Return_True_If_NameIsNoEmpty()
        {
            // Arrange.
            var newEmpCommand = new AddEmployeeCommand { DepartmentId = _departmentList.First().Id, Name = "Some Name", Phone = "1234567" };

            // Act.
            var validation = _addEmployeeValidator.Validate(newEmpCommand);

            // Assert.
            validation.IsValid.Should().BeTrue();
        }

        [TestMethod()]
        public void AddEmployeeValidatorTests_Should_Return_False_If_NameIsEmpty()
        {
            // Arrange.
            var newEmpCommand = new AddEmployeeCommand { DepartmentId = _departmentList.First().Id, Name = null!, Phone = "1234567" };

            // Act.
            var validation = _addEmployeeValidator.Validate(newEmpCommand);

            // Assert.
            validation.IsValid.Should().BeFalse();
        }

        [TestMethod()]
        public void AddEmployeeValidatorTests_Should_Return_True_If_DepartmentIsFound()
        {
            // Arrange.
            var newEmpCommand = new AddEmployeeCommand { DepartmentId = _departmentList.First().Id, Name = "Some Name", Phone = "1234567" };

            // Act.
            var validation = _addEmployeeValidator.Validate(newEmpCommand);

            // Assert.
            validation.IsValid.Should().BeTrue();
        }

        [TestMethod()]
        public void AddEmployeeValidatorTests_Should_Return_False_If_DepartmentIsNotFound()
        {
            // Arrange.
            var newEmpCommand = new AddEmployeeCommand { DepartmentId = "Unknown", Name = "Some Name", Phone = "1234567" };

            // Act.
            var validation = _addEmployeeValidator.Validate(newEmpCommand);

            // Assert.
            validation.IsValid.Should().BeFalse();
        }
    }
}