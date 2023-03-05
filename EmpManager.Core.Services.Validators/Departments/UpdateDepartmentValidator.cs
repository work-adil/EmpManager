using EmpManager.Core.Services.CQRS.Commands.Departments;
using FluentValidation;

namespace EmpManager.Core.Services.Validators.Departments
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
