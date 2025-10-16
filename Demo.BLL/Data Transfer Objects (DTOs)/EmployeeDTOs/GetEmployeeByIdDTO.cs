using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.Shared;

namespace Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs
{
    public class GetEmployeeByIdDTO 
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        public int Age { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [Range(0, 10000000.00, ErrorMessage = "Salary cannot exceed 10,000,000.00")]
        public decimal Salary { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HiringDate { get; set; }
        public EmployeeGender gender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public int? DepartmentID { get; set; }
        public string? Department { get; set; }
        public string? ImageName { get; set; }
        public List<DailyAttendanceDTO> DailyAttendances { get; set; } = new();
        public string? CvFileName { get; set; }
    }
}
