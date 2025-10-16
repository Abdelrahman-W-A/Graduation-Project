using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Data.Repostitories.EntityTypes;
using Demo.DAL.Models.IDentityModel;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Data.Repostitories.NoUsedRepo.Users
{
    public class UsersRepo(ApplicationDbContext _dbContext) : IUserRepo
    {
        public Application_User? GetById(string id) 
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<Application_User> GetAll(bool WithTracking = false) 
        {
            if (WithTracking)
                return _dbContext.Set<Application_User>().ToList();
            else
                return _dbContext.Set<Application_User>().AsNoTracking().ToList();
        }

        public int Update(Application_User user)
        {
            var trackedEntity = _dbContext.Users.Attach(user);
            trackedEntity.State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }
        public int Remove(Application_User department) 
        {
            _dbContext.Set<Application_User>().Remove(department);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Application_User> GetAll(Expression<Func<Application_User, bool>> predicate)
        {
            return _dbContext.Set<Application_User>().Where(predicate).ToList();
        }

        public IEnumerable<Application_User> GetAll()
        {
            return _dbContext.Set<Application_User>().ToList();
        }
    }

}
