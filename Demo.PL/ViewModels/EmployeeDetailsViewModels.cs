using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.DAL.Models.AttendanceModel;
using System.Collections.Generic;

namespace Demo.PL.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public GetEmployeeByIdDTO Employee { get; set; }
        public List<Attendance> AttendanceRecords { get; set; }
    }
}