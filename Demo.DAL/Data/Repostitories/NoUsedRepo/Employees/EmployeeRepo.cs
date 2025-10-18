using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Data.Repostitories.EntityTypes;
using Demo.DAL.Models.EmployeeModel;

namespace Demo.DAL.Data.Repostitories.NoUsedRepo.Employees
{
    public class EmployeeRepo(ApplicationDbContext context) : EntityTypeCrudImplementations<Employee>(context), IEmployeeRepo
    {

    }
}
