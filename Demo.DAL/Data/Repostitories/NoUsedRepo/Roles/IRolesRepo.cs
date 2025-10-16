using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.DepartmentModel;
using Demo.DAL.Models.IDentityModel;

namespace Demo.DAL.Data.Repostitories.NoUsedRepo.Roles
{
    public interface IRolesRepo
    {
        IEnumerable<Demo.DAL.Models.IDentityModel.Roles> GetAll(bool WithTracking = false);
        IEnumerable<Demo.DAL.Models.IDentityModel.Roles> GetAll();
        Demo.DAL.Models.IDentityModel.Roles? GetById(string id);
        int Remove(Demo.DAL.Models.IDentityModel.Roles User);
        int Update(Demo.DAL.Models.IDentityModel.Roles User);
        int Add(Models.IDentityModel.Roles department);

    }
}
