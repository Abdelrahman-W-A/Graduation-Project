using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models.Shared
{
    public class BaseEntity
    {

        #region BaseEntity Properties
        public int Id { get; set; } 
        public int CreatedBy { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public int ModifiedBy { get; set; } 
        public DateTime? ModifiedOn { get; set; } 
        public bool IsDeleted { get; set; } 
        #endregion

    }
}
