using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.DepartmentModel;
using Demo.DAL.Models.IDentityModel;

namespace Demo.DAL.Data.Repostitories.NoUsedRepo.Users
{
    public interface IUserRepo
    {
        IEnumerable<Application_User> GetAll(bool WithTracking = false);
        IEnumerable<Application_User> GetAll();
        Application_User? GetById(string id);
        int Remove(Application_User User);
        int Update(Application_User User);
    }
}
