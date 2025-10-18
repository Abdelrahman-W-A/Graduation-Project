using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs
{
    public class DailyAttendanceDTO
    {
        public int Id { get; set; }          
        public int EmployeeId { get; set; }  
        public DateTime Date { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public bool IsAbsent { get; set; }
    }
}
