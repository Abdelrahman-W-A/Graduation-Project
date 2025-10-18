using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.DepartmentModel;
using Demo.DAL.Models.AttendanceModel;
using Demo.DAL.Models.Shared;

namespace Demo.DAL.Models.EmployeeModel
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Role { get; set; }
        public int age { get; set; }
        public string? Address { get; set; }
        public Decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HiringDate { get; set;}
        public EmployeeGender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public int? DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
        public string? ImageName { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public string? CvFileName { get; set; }
    }
}
