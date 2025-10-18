using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.DAL.Models.AttendanceModel;

namespace Demo.BLL.Services.AttendanceServices
{
    public interface IAttendanceService
    {
        int AddAttendance(DailyAttendanceDTO dto);
        void UpdateAttendance(DailyAttendanceDTO dto);
        bool DeleteAttendance(int id);
        DailyAttendanceDTO? GetById(int id);
        IEnumerable<DailyAttendanceDTO> GetByEmployeeId(int employeeId);
    }
}

