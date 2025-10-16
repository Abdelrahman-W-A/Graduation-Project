using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.BLL.Services.AttendanceServices;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        #region Index
        [HttpGet]
        public IActionResult Index(int employeeId)
        {
            var attendances = _attendanceService.GetByEmployeeId(employeeId);

            ViewBag.EmployeeId = employeeId;

            return View(attendances);
        }
        #endregion

        #region Create
        //// Create GET
        //[HttpGet]
        //public IActionResult Create(int employeeId)
        //{
        //    var model = new DailyAttendanceDTO
        //    {
        //        EmployeeId = employeeId,
        //        Date = DateTime.Today
        //    };
        //    return View(model);
        //}

        //// Create POST
        //[HttpPost]
        //public IActionResult Create(DailyAttendanceDTO model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    _attendanceService.AddAttendance(model);
        //    return RedirectToAction("Index", new { employeeId = model.EmployeeId });
        //}

        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dto = _attendanceService.GetById(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost]
        public IActionResult Edit(DailyAttendanceDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _attendanceService.UpdateAttendance(model);
            return RedirectToAction("Index", new { employeeId = model.EmployeeId });
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dto = _attendanceService.GetById(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id, int employeeId)
        {
            _attendanceService.DeleteAttendance(id);
            return RedirectToAction("Index", new { employeeId = employeeId });
        }
        #endregion

    }

}
