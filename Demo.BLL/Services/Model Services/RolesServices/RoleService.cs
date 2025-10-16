using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Demo.BLL.Data_Transfer_Objects__DTOs_.RolesDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;
using Demo.BLL.Services.Model_Services.UserServices;
using Demo.DAL.Data.Repostitories.NoUsedRepo.Roles;
using Demo.DAL.Data.Repostitories.NoUsedRepo.Users;
using Demo.DAL.Models.IDentityModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Demo.BLL.Services.Model_Services.RolesServices
{
    public class RoleService(IRolesRepo _usersRepo) : IRolesServices
    {
        public int AddRole(string roleName)
        {

            // Create a new role DTO
            var randomRoleName = $"Role-{Guid.NewGuid().ToString().Substring(0, 8)}";

            // Map CreatedRoleDTO to Roles entity
            var newRoleEntity = new Roles
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = randomRoleName
            };

            // Add the role using the repository
            var result = _usersRepo.Add(newRoleEntity);

            if (result > 0)
            {
                // Redirect to the roles list
                Console.WriteLine("Role Created succefully");
            }
            else
            {
                Console.WriteLine("Failed to add role.");
            }

            return result;
        }

        public bool DeleteRole(string id)
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

        public IEnumerable<CreatedRoleDTO> GetAllRoles()
        {
            var users = _usersRepo.GetAll();

            var usersToReturn = users.Select(U => new CreatedRoleDTO()
            {
                Id = U.Id,
                RoleName = U.RoleName
            });
            return usersToReturn;
        }

        public RoleDetailsDTO? GetRoleById(string id)
        {
            var users = _usersRepo.GetById(id);

            if (users is null) return null;
            else
            {
                var UsersToReturn = new RoleDetailsDTO()
                {
                    Id = users.Id,
                    RoleName = users.RoleName
                };
                return UsersToReturn;
            }
        }

        public int UpdateRole(RoleEditDTO updatedUser)
        {
            var Role = _usersRepo.GetById(updatedUser.Id);
            return _usersRepo.Update(Role);
        }
    }
}
