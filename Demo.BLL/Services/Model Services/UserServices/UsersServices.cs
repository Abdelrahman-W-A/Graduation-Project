using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTO_;
using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;
using Demo.BLL.Factories.UserFactory;
using Demo.DAL.Data.Repostitories.NoUsedRepo.Users;
using Demo.DAL.Models.IDentityModel;

namespace Demo.BLL.Services.Model_Services.UserServices
{
    public class UsersServices(IUserRepo _usersRepo) : IUsersServices
    {
        public bool DeleteUser(string id)
        {
            var user = _usersRepo.GetById(id);
            if (user is null) return false;
            else
            {
                int result = _usersRepo.Remove(user);
                if (result > 0) return true;
                else return false;
            }
        }

        public IEnumerable<CreatedUserDTO> GetAllUsers()
        {
            var users = _usersRepo.GetAll();

            var usersToReturn = users.Select(U => new CreatedUserDTO()
            {
                Id = U.Id,
                FName = U.FirstName,
                LName = U.LastName,
                Email = U.Email,
                PhoneNumber = U.PhoneNumber
                //Role = U.Role
            });

            return usersToReturn;
        }

        public UsersDetails? GetUserById(string id)
        {
            var users = _usersRepo.GetById(id);

            if (users is null) return null;
            else
            {
                var UsersToReturn = new UsersDetails()
                {
                    Id = users.Id,
                    FName = users.FirstName,
                    LName = users.LastName,
                    Email = users.Email,
                    PhoneNumber = users.PhoneNumber
                    //Role = users.Role
                };
                return UsersToReturn;
            }
        }

        public int UpdateUser(UsersUpdatedDTO updatedUser)
        {
            return _usersRepo.Update(updatedUser.ToEntity());
        }
    }
}
