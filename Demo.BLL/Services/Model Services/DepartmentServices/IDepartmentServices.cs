using Demo.BLL.Data_Transfer_Objects__DTO_;
using Demo.BLL.Data_Transfer_Objects__DTOs_;

namespace Demo.BLL.Services.DepartmentServices
{
    public interface IDepartmentServices
    {
        int AddDepartment(CreatedDepartmentDTO createdDepartment);
        bool DeleteDepartment(int id);
        IEnumerable<DepartmentDTO> GetAllDepartments();
        DepartmentDetailsDTO? GetDepartmentById(int id);
        int UpdateDepartment(UpdatedDepartmentDTO updatedDepartment);
    }
}