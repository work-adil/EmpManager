using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using FluentAssertions;

namespace EmpManager.Core.Services.Validators.Departments.Tests
{
    [TestClass()]
    public class AddDepartmentValidatorTests
    {
        private readonly AddDepartmentValidator _addDepartmentValidator;
        private readonly List<Department> _departmentList = new List<Department>();
        public AddDepartmentValidatorTests()
        {
            var repoMock = new Mock<IGenericRepository<Department>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), default)).ReturnsAsync((string id, CancellationToken ct) => _departmentList.FirstOrDefault(x => x.Id == id));
            _addDepartmentValidator = new AddDepartmentValidator();
        }

        [TestMethod()]
        public void AddDepartmentValidatorTests_Should_Return_True_If_NameIsNotEmpty()
        {
            // Arrange.
            var newDeptCommand = new AddDepartmentCommand {Name = "Some Name" };

            // Act.
            var validation = _addDepartmentValidator.Validate(newDeptCommand);

            // Assert.
            validation.IsValid.Should().BeTrue();
        }

        [TestMethod()]
        public void AddDepartmentValidatorTests_Should_Return_False_If_NameIsEmpty()
        {
            // Arrange.
            var newDeptCommand = new AddDepartmentCommand { Name = "" };

            // Act.
            var validation = _addDepartmentValidator.Validate(newDeptCommand);

            // Assert.
            validation.IsValid.Should().BeFalse();
        }
    }
}