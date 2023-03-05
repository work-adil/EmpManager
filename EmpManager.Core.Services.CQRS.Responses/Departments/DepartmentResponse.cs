namespace EmpManager.Core.Services.CQRS.Responses.Departments
{
    public class DepartmentResponse : ModelResponseBase
    {
        /// <summary>
        /// Name of the department.
        /// </summary>
        public required string Name { get; set; }
    }
}