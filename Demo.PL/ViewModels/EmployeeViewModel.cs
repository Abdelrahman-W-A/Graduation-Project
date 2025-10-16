using Demo.DAL.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Range(0, 10000000.00, ErrorMessage = "Salary cannot exceed 10,000,000.00")]
        public decimal Salary { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HiringDate { get; set; }
        public EmployeeGender gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int? DepartmentID { get; set; }
        public IFormFile? Image { get; set; }
        public string? CapturedImage { get; set; }
        public int TotalSalaries { get; set; }
        public string? ExistingImageName { get; set; }
        public IFormFile? CvFile { get; set; }   // For uploading new CV
        public string? ExistingCvFileName { get; set; } // To store old CV
        public string? CvFileName { get; set; }  // stored CV filename in DB

    }
}
