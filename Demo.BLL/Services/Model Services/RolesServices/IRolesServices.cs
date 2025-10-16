
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.RolesDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;

namespace Demo.BLL.Services.Model_Services.RolesServices
{
    public interface IRolesServices
    {
        bool DeleteRole(string id);
        IEnumerable<CreatedRoleDTO> GetAllRoles();
        RoleDetailsDTO? GetRoleById(string id);
        int UpdateRole(RoleEditDTO updatedUser);
        int AddRole(string RoleName);

    }
}
