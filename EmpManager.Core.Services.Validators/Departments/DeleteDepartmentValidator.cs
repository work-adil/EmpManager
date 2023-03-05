using EmpManager.Core.Domain.Models;
using EmpManager.Core.Domain;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using FluentValidation;

namespace EmpManager.Core.Services.Validators.Departments
{
    public class DeleteDepartmentValidator : AbstractValidator<DeleteDepartmentCommand>
    {
        public DeleteDepartmentValidator(IGenericRepository<Employee> employeeRepository)
        {

            RuleFor(x => x.Id).NotEmpty()
                .Must((request, departmentId) => employeeRepository.GetQueryable().Any(x=>x.DepartmentId == departmentId))
                .WithMessage("Some employees are already under this department.");
        }
    }
}
