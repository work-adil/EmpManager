using EmpManager.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpManager.Infrastructure.RelationalDb
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var itDepartment = new Department { Name = "IT", Id = "IT-Department-ID" };
            modelBuilder.Entity<Department>().HasData(new[] { itDepartment, new Department { Name = "Operation", Id = "OP-Department-ID" } });
            modelBuilder.Entity<Employee>().HasData(new[] { new Employee { DepartmentId = itDepartment.Id, Name = "Adil", Phone = "007" } });

            base.OnModelCreating(modelBuilder);
        }
    }
}
