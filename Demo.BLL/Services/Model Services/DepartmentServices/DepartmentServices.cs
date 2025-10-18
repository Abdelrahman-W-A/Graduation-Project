using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTO_;
using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Factories.DepartmentsFactory;
using Demo.DAL.Data.Repostitories.EntityTypes;
using Demo.DAL.Data.Repostitories.NoUsedRepo.Departments;
using Demo.DAL.Models;
using Demo.DAL.Models.EmployeeModel;
namespace Demo.BLL.Services.DepartmentServices
{
    public class DepartmentServices(IDepartmentRepostiory _departmentRepostiory, IEntityTypeRepo<Employee> entityTypeRepo) : IDepartmentServices
    {

        public IEnumerable<DepartmentDTO> GetAllDepartments()
        {
            var departments = _departmentRepostiory.GetAll();

            var departmentToReturn = departments.Select(d => new DepartmentDTO()
            {
                DeptID = d.Id,
                Name = d.Name,
                Code = d.Code,
                Description = d.Description ?? "No Description",
                DateOfCreation = DateOnly.FromDateTime((DateTime)d.CreatedOn!),

                // Count employees in this department
                EmployeeCount = entityTypeRepo.GetAll().Count(e => e.DepartmentID == d.Id)
            });

            return departmentToReturn;
        }


        public DepartmentDetailsDTO? GetDepartmentById(int id)
        {
            var dept = _departmentRepostiory.GetById(id);
            if (dept == null) return null;

            var employeeCount = entityTypeRepo.GetAll()
                                .Count(e => e.DepartmentID == id);

            return new DepartmentDetailsDTO
            {
                Id = dept.Id,
                Code = dept.Code,
                Name = dept.Name,
                Description = dept.Description,
                CreatedOn = DateOnly.FromDateTime((DateTime)dept.CreatedOn!),
                EmployeeCount = employeeCount
            };
        }


        public int AddDepartment(CreatedDepartmentDTO createdDepartment)
        {
            var department = createdDepartment.ToEntity();
            return _departmentRepostiory.Add(department);
        }

        public int UpdateDepartment(UpdatedDepartmentDTO updatedDepartment)
        {
            return _departmentRepostiory.Update(updatedDepartment.ToEntity());
        }

        public bool DeleteDepartment(int id)
        {
            var dept = _departmentRepostiory.GetById(id);
            if (dept is null) return false;
            else
            {
                int result = _departmentRepostiory.Remove(dept);
                if (result > 0) return true;
                else return false;
            }
        }
    }
}
