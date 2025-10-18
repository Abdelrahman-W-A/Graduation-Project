using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.DAL.Models.EmployeeModel;

namespace Demo.BLL.Factories.EmployeesFactory
{
    static class EmployeeFactory
    {
        public static Employee ToEntity(this GetEmployeeDTO employeeDTO)
        {
            return new Employee()
            {
                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                age = employeeDTO.Age,
                IsActive = employeeDTO.IsActive,
                Salary = employeeDTO.Salary,
                Email = employeeDTO.Email,
                Gender = employeeDTO.gender,
                EmployeeType = employeeDTO.EmployeeType
            };
        }

        public static Employee ToEntity(this UpdatedEmployeeDTO employeeDTO)
        {
            return new Employee()
            {
                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                age = employeeDTO.Age,
                IsActive = employeeDTO.IsActive,
                Salary = employeeDTO.Salary,
                Email = employeeDTO.Email,
                PhoneNumber = employeeDTO.PhoneNumber,
                HiringDate = employeeDTO.HiringDate.ToDateTime(new TimeOnly()),
                Gender = employeeDTO.gender,
                EmployeeType = employeeDTO.EmployeeType,
                CreatedBy = employeeDTO.CreatedBy,
                ModifiedBy = employeeDTO.LastModifiedBy
            };
        }

        public static Employee ToEntity(this AddNewEmployeeDTO employeeDTO)
        {
            return new Employee()
            {
                Name = employeeDTO.Name,
                Email = employeeDTO.Email,
                PhoneNumber = employeeDTO.PhoneNumber,
                age = employeeDTO.Age,
                HiringDate = employeeDTO.HiringDate.ToDateTime(new TimeOnly()),
                Salary = employeeDTO.Salary,
                EmployeeType = employeeDTO.EmployeeType,
                IsActive = employeeDTO.IsActive,
                Gender = employeeDTO.gender
            };
        }
    }
}
