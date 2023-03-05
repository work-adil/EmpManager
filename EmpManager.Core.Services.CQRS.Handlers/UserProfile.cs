using AutoMapper;
using EmpManager.Core.Domain.Models;
using EmpManager.Core.Services.CQRS.Commands.Departments;
using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.CQRS.Responses.Departments;
using EmpManager.Core.Services.CQRS.Responses.Employees;

namespace EmpManager.Core.Services.CQRS.Handlers
{
    /// <summary>
    /// For automapping by AutoMapper.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Create maps entities here.
        /// </summary>
        public UserProfile()
        {
            // Employee
            CreateMap<Employee, EmployeeResponse>().ReverseMap();
            CreateMap<AddEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();

            // Department
            CreateMap<Department, DepartmentResponse>();
            CreateMap<AddDepartmentCommand, Department>();
            CreateMap<UpdateDepartmentCommand, Department>();
        }
    }
}
