using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Models.DepartmentModel;

namespace Demo.DAL.Data.Repostitories.NoUsedRepo.Departments
{
    public class DepartmentRepostiory : EntitytypeCrudImplementations<Department>, IDepartmentRepostiory
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepostiory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Department department)
        {
            _dbContext.Departments.Add(department);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll(bool WithTracking = false)
        {
            if (WithTracking)
            {
                return _dbContext.Departments.ToList();
            }
            else
            {
                return _dbContext.Departments.AsNoTracking().ToList();
            }
        }

        public Department? GetById(int id)
        {
            return _dbContext.Departments.Find(id);
        }

        public int Remove(Department department)
        {
            _dbContext.Departments.Remove(department);
            return _dbContext.SaveChanges();
        }

        public int Update(Department department)
        {
            _dbContext.Departments.Update(department);
            return _dbContext.SaveChanges();
        }
    }
}

