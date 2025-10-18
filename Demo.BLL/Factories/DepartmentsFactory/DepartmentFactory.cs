using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.DAL.Models.DepartmentModel;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Demo.BLL.Factories.DepartmentsFactory
{
    static class DepartmentFactory
    {
        public static Department ToEntity(this CreatedDepartmentDTO departmentDTO)
        {
            return new Department()
            {
                Name = departmentDTO.Name,
                Code = departmentDTO.Code,
                Description = departmentDTO.Description,
                CreatedOn = departmentDTO.DateOfCreation.ToDateTime(new TimeOnly())
            };
        }

        public static Department ToEntity(this UpdatedDepartmentDTO updatedDepartment)
        {
            return new Department()
            {
                Id = updatedDepartment.ID,
                Name = updatedDepartment.Name,
                Code = updatedDepartment.code,
                CreatedOn = updatedDepartment.CreatedDate.ToDateTime(new TimeOnly()),
                Description = updatedDepartment.Description
            };
        }
    }
}
