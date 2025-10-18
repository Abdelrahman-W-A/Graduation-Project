using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.DepartmentModel;
using Demo.DAL.Models.Shared;

namespace Demo.DAL.Data.Repostitories.EntityTypes
{
    public interface IEntityTypeRepo<T>
    {
        int Add(T department);
        IEnumerable<T> GetAll(bool WithTracking = false);
        IEnumerable<T> GetAll(Expression<Func<T,bool>> predicate);
        T? GetById(int id);
        int Remove(T department);
        int Update(T department);
    }
}
