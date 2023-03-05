using EmpManager.Core.Domain;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using FluentValidation;

namespace EmpManager.Core.Services.Validators.Employees
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeValidator(IGenericRepository<Department> departmentRepository)
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.DepartmentId).NotEmpty()
                .Must((request, departmentId, c) =>
                {
                    return departmentRepository.GetByIdAsync(departmentId).GetAwaiter().GetResult() != null;
                }).WithMessage("No department found with the DepartmentId provided.");
        }
    }
}
