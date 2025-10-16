using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.EmployeeModel;
using Demo.DAL.Models.Shared;

namespace Demo.DAL.Models.DepartmentModel
{
    public class Department : BaseEntity
    {

        #region Properties
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
         public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();   
        #endregion

    }
}
