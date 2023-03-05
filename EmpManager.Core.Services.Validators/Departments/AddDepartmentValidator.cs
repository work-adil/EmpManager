using EmpManager.Core.Services.CQRS.Commands.Departments;
using FluentValidation;

namespace EmpManager.Core.Services.Validators.Departments
{
    public class AddDepartmentValidator : AbstractValidator<AddDepartmentCommand>
    {
        public AddDepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
