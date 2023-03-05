namespace EmpManager.Core.Domain.Models
{
    public class Employee : ModelBase
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
        public virtual Department Department { get; set; } = null!;

        /// <summary>
        /// Email.
        /// </summary>
        public required string Email { get; set; }

    }
}
