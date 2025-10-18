using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Models.IDentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Data.Repostitories.NoUsedRepo.Roles
{
    public class RolesRepo(ApplicationDbContext _dbContext) : IRolesRepo
    {
        public Models.IDentityModel.Roles? GetById(string id)
        {
            return _dbContext.Set<Models.IDentityModel.Roles>().FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Models.IDentityModel.Roles> GetAll(bool WithTracking = false)
        {
            if (WithTracking)
                return _dbContext.Set<Models.IDentityModel.Roles>().ToList();
            else
                return _dbContext.Set<Models.IDentityModel.Roles>().AsNoTracking().ToList();
        }

        public int Update(Models.IDentityModel.Roles role)
        {
            var trackedEntity = _dbContext.Set<Models.IDentityModel.Roles>().Attach(role);
            trackedEntity.State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }
        public int Remove(Models.IDentityModel.Roles roles)
        {
            _dbContext.Set<Models.IDentityModel.Roles>().Remove(roles);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Models.IDentityModel.Roles> GetAll(Expression<Func<Models.IDentityModel.Roles, bool>> predicate)
        {
            return _dbContext.Set<Models.IDentityModel.Roles>().Where(predicate).ToList();
        }

        public IEnumerable<Models.IDentityModel.Roles> GetAll()
        {
            return _dbContext.Set<Models.IDentityModel.Roles>().ToList();
        }


        public int Add(Models.IDentityModel.Roles roles)
        {
            var identityRole = new IdentityRole
            {
                Id = roles.Id,
                Name = roles.RoleName,
                //NormalizedName = roles.RoleName?.ToUpperInvariant(),
                //ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Add the IdentityRole to the IdentityRole DbSet
            _dbContext.Set<IdentityRole>().Add(identityRole);

            // Add the Roles entity to the Roles DbSet
            _dbContext.Set<Models.IDentityModel.Roles>().Add(roles);

            return _dbContext.SaveChanges();
        }
    
    }
}
