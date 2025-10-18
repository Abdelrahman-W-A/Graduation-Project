using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTO_;
using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Data_Transfer_Objects__DTOs_.DepartmentDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;

namespace Demo.BLL.Services.EmployeeServices
{
    public interface IEmployeeServices
    {
        int AddEmployee(AddNewEmployeeDTO createdEmployee);
        bool DeleteEmployee(int id);
        IEnumerable<GetEmployeeDTO> GetAllEmployees(string? Name);
        GetEmployeeByIdDTO? GetEmployeeById(int id);
        int UpdateEmployee(UpdatedEmployeeDTO updatedEmployee);

    }
}
