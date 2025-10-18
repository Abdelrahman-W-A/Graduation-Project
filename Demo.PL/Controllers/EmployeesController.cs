using System.Text.RegularExpressions;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.BLL.Services.AttendanceServices;
using Demo.BLL.Services.DepartmentServices;
using Demo.BLL.Services.EmployeeServices;
using Demo.DAL.Models.EmployeeModel;
using Demo.DAL.Models.Shared;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeesController(IEmployeeServices _employeeServices, IAttendanceService attendanceService, IWebHostEnvironment environment, ILogger<EmployeesController> logger, EmployeePdfService employeePdfService) : Controller
    {

        #region Index
        [HttpGet]
        public IActionResult Index(string? EmployeeSearchName)
        {
            var employees = _employeeServices.GetAllEmployees(EmployeeSearchName);

            // total of all salaries
            var totalSalaries = employees.Sum(e => e.Salary);

            // pass it to the view
            ViewBag.TotalSalaries = totalSalaries;

            return View(employees);
        }


        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new AddNewEmployeeDTO
                {
                    Name = model.Name,
                    Role = model.Role,
                    Address = model.Address,
                    Age = model.Age,
                    IsActive = model.IsActive,
                    Salary = model.Salary,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    HiringDate = model.HiringDate,
                    gender = model.gender,
                    EmployeeType = model.EmployeeType,
                    DepartmentID = model.DepartmentID,
                    Image = model.Image,        // ← مهم
                    CvFile = model.CvFile       // ← مهم
                };

                _employeeServices.AddEmployee(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }








        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int id, DateTime? fromDate, DateTime? toDate)
        {
            var employee = _employeeServices.GetEmployeeById(id);
            if (employee == null)
                return NotFound();

            var attendances = attendanceService.GetByEmployeeId(id);

            // Apply date filtering
            if (fromDate.HasValue && toDate.HasValue)
            {
                attendances = attendances
                    .Where(a => a.Date >= fromDate.Value && a.Date <= toDate.Value)
                    .ToList();
            }

            employee.DailyAttendances = attendances
                                        .Select(a => new DailyAttendanceDTO
                                        {
                                            Id = a.Id,
                                            EmployeeId = a.EmployeeId,
                                            Date = a.Date,
                                            CheckIn = a.CheckIn,
                                            CheckOut = a.CheckOut,
                                            IsAbsent = a.IsAbsent
                                        }).ToList();

            // Pass filter values back to view
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(employee);
        }


        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();

            var employeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Salary = employee.Salary,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                HiringDate = employee.HiringDate,
                gender = employee.gender,
                EmployeeType = employee.EmployeeType,
                DepartmentID = employee.DepartmentID,
                Age = employee.Age,
                Address = employee.Address,
                Role = employee.Role,
                ExistingImageName = employee.ImageName,         // ✅ عشان الصورة
                ExistingCvFileName = employee.CvFileName    // ✅ عشان الـ CV
            };
            return View(employeeViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdatedEmployeeDTO
            {
                Id = model.Id,
                Name = model.Name,
                Salary = model.Salary,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = model.IsActive,
                HiringDate = model.HiringDate,
                gender = model.gender,
                EmployeeType = model.EmployeeType,
                DepartmentID = model.DepartmentID,
                Age = model.Age,
                Address = model.Address,
                Role = model.Role,
                Image = model.Image,                 // ✅ لو رفع صورة جديدة
                CvFile = model.CvFile,               // ✅ لو رفع CV جديد
                ImageName = model.ExistingImageName, // ✅ لو ما غيرش الصورة
                CvFileName = model.ExistingCvFileName, // ✅ لو ما غيرش CV
                CapturedImage = model.CapturedImage
            };

            var result = _employeeServices.UpdateEmployee(dto);

            if (result > 0)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to update employee.");
            return View(model);
        }


        #endregion

        #region Delete

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == 0) return BadRequest();
            try
            {
                var deleted = _employeeServices.DeleteEmployee(id!.Value);
                if (deleted) return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee is not deleted");
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            catch (Exception EX)
            {
                if (environment.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
                else
                {
                    logger.LogError(EX.Message);
                    return View("ErrorView", EX);
                }
            }
        }

        #endregion

        #region Attendance
        [HttpGet]
        public IActionResult AddAttendance(int employeeId)
        {
            var model = new AddAttendanceViewModel
            {
                EmployeeId = employeeId,
                Date = DateTime.Now
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult AddAttendance(AddAttendanceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new DailyAttendanceDTO
            {
                EmployeeId = model.EmployeeId,
                Date = model.Date,
                CheckIn = model.CheckIn,
                CheckOut = model.CheckOut,
                IsAbsent = model.IsAbsent
            };

            attendanceService.AddAttendance(dto);

            return RedirectToAction("Details", new { id = model.EmployeeId });
        }

        // Create
        [HttpGet]
        public IActionResult CreateAttendance(int employeeId)
        {
            var model = new AddAttendanceViewModel { EmployeeId = employeeId, Date = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateAttendance(AddAttendanceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new DailyAttendanceDTO
            {
                EmployeeId = model.EmployeeId,
                Date = model.Date,
                CheckIn = model.CheckIn,
                CheckOut = model.CheckOut,
                IsAbsent = model.IsAbsent
            };

            attendanceService.AddAttendance(dto);

            return RedirectToAction("Details", new { id = model.EmployeeId });
        }

        // Edit
        [HttpGet]
        public IActionResult EditAttendance(int id)
        {
            var attendance = attendanceService.GetById(id);
            if (attendance == null) return NotFound();

            var model = new AddAttendanceViewModel
            {
                Id = attendance.Id,
                EmployeeId = attendance.EmployeeId,
                Date = attendance.Date,
                CheckIn = attendance.CheckIn,
                CheckOut = attendance.CheckOut,
                IsAbsent = attendance.IsAbsent
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditAttendance(AddAttendanceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new DailyAttendanceDTO
            {
                Id = model.Id,
                EmployeeId = model.EmployeeId,
                Date = model.Date,
                CheckIn = model.CheckIn,
                CheckOut = model.CheckOut,
                IsAbsent = model.IsAbsent
            };

            attendanceService.UpdateAttendance(dto);

            return RedirectToAction("Details", new { id = model.EmployeeId });
        }

        // Delete
        [HttpPost]
        public IActionResult DeleteAttendance(int id)
        {
            var att = attendanceService.GetById(id);
            if (att != null)
            {
                attendanceService.DeleteAttendance(id);
                return RedirectToAction("Details", new { id = att.EmployeeId });
            }
            return NotFound();
        }


        #endregion

        #region PDF
        public IActionResult ExportToPdf(int id)
        {
            var employeeDto = _employeeServices.GetEmployeeById(id);
            if (employeeDto == null) return NotFound();

            // Map DTO to ViewModel
            var employeeViewModel = new EmployeeViewModel
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                Role = employeeDto.Role,
                HiringDate = employeeDto.HiringDate,
                Salary = employeeDto.Salary,
                IsActive = employeeDto.IsActive,
                DepartmentID = employeeDto.DepartmentID,
                gender = employeeDto.gender,
                ExistingImageName = employeeDto.ImageName
            };

            var pdfBytes = employeePdfService.GenerateEmployeePdf(employeeViewModel);

            return File(pdfBytes, "application/pdf", $"{employeeViewModel.Name}_Details.pdf");
        }
        #endregion

        #region CV
        public IActionResult DownloadCv(int id)
        {
            var employee = _employeeServices.GetEmployeeById(id);
            if (employee == null || string.IsNullOrEmpty(employee.CvFileName))
                return NotFound("CV not available.");

            var filePath = Path.Combine(environment.WebRootPath, "Files/CVs", employee.CvFileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("CV file not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", $"{employee.Name}_CV.pdf");
        }

        private readonly string[] allowedExtensions = new[] { ".pdf", ".docx", ".png", ".jpg", ".jpeg" };
        private const long MaxSize = 10 * 1024 * 1024; // 10 MB

        public string? Upload(IFormFile file, string folderName)
        {
            // 1. Check Extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return null;

            // 2. Check Size
            if (file.Length > MaxSize || file.Length == 0)
                return null;

            // 3. Get full folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);

            // 4. Ensure directory exists
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 5. Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            // 6. Save file
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }

        #endregion

    }
}
    

