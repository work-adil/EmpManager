using EmpManager.Core.Services.CQRS.Responses.Departments;

namespace EmpManager.Core.Services.CQRS.Responses.Employees
{
    public class EmployeeResponse : ModelResponseBase
    {
        /// <summary>
        /// Name of the employee.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Id of the Department.
        /// </summary>
        public required string DepartmentId { get; set; }

        /// <summary>
        /// Department (Navigational Property).
        /// </summary>
        public virtual DepartmentResponse Department { get; set; } = null!;

        /// <summary>
        /// Phone number.
        /// </summary>
        public required string Phone { get; set; }

    }
}