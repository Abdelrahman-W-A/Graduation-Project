using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;
using Demo.DAL.Models.IDentityModel;

namespace Demo.BLL.Services.Model_Services.UserServices
{
    public interface IUsersServices
    {
        bool DeleteUser(string id);
        //IEnumerable<CreatedUserDTO> GetAllUsers(string? UserSearchName);
        IEnumerable<CreatedUserDTO> GetAllUsers();
        UsersDetails? GetUserById(string id);
        int UpdateUser(UsersUpdatedDTO updatedUser);
    }
}
