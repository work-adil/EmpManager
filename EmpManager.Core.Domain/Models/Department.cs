namespace EmpManager.Core.Domain.Models
{
    public class Department : ModelBase
    {
        /// <summary>
        /// Name of the department.
        /// </summary>
        public required string Name { get; set; }

    }
}