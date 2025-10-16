using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;
using Demo.DAL.Models.DepartmentModel;
using Demo.DAL.Models.IDentityModel;

namespace Demo.BLL.Factories.UserFactory
{
    public static class UserFactory
    {
        public static Application_User ToEntity(this UsersUpdatedDTO createdUserDTO)
        {
            return new Application_User()
            {
                Id = createdUserDTO.Id,
                FirstName = createdUserDTO.FName,
                LastName = createdUserDTO.LName,
                PhoneNumber = createdUserDTO.PhoneNumber
            };
        }
    }
}
