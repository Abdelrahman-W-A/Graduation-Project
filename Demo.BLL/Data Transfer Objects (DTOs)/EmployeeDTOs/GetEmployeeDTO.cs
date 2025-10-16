using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.Shared;

namespace Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs
{
    public class GetEmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public int Age { get; set; }
        [Display (Name = "Is Active")]
        public bool IsActive { get; set; }
        [DataType (DataType.Currency)]
        [Range(0, 10000000.00, ErrorMessage = "Salary cannot exceed 10,000,000.00")]
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public EmployeeGender gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }

        [Display(Name = "Department")]
        public string? Department { get; set; }
    }
}
