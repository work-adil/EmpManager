using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using FluentAssertions;

namespace EmpManager.Core.Services.Validators.Departments.Tests
{
    [TestClass()]
    public class UpdateDepartmentValidatorTests
    {
        private readonly UpdateDepartmentValidator _UpdateDepartmentValidator;
        private readonly List<Department> _departmentList = new List<Department>();
        public UpdateDepartmentValidatorTests()
        {
            var repoMock = new Mock<IGenericRepository<Department>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), default)).ReturnsAsync((string id, CancellationToken ct) => _departmentList.FirstOrDefault(x => x.Id == id));
            _UpdateDepartmentValidator = new UpdateDepartmentValidator();
        }

        [TestMethod()]
        public void UpdateDepartmentValidatorTests_Should_Return_True_If_NameIsNotEmpty()
        {
            // Arrange.
            var newDeptCommand = new UpdateDepartmentCommand {Name = "Some Name" };

            // Act.
            var validation = _UpdateDepartmentValidator.Validate(newDeptCommand);

            // Assert.
            validation.IsValid.Should().BeTrue();
        }

        [TestMethod()]
        public void UpdateDepartmentValidatorTests_Should_Return_False_If_NameIsEmpty()
        {
            // Arrange.
            var newDeptCommand = new UpdateDepartmentCommand { Name = "" };

            // Act.
            var validation = _UpdateDepartmentValidator.Validate(newDeptCommand);

            // Assert.
            validation.IsValid.Should().BeFalse();
        }
    }
}