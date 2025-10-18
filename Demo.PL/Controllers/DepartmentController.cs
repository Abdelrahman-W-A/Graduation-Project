using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Services.DepartmentServices;
using Demo.DAL.Models;
using Demo.PL.DepartmentViewModels;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Demo.DAL.Data.Repostitories.EntityTypes;
using Demo.DAL.Models.EmployeeModel;

namespace Demo.PL.Controllers
{
    public class DepartmentController(IDepartmentServices _departmentServices, ILogger<DepartmentController> _logger, IWebHostEnvironment _webHostEnvironment, IEntityTypeRepo<Employee> _employeeRepo) : Controller
    {

        #region Show Department
        [HttpGet]
        public IActionResult Index()
        {
            var dept = _departmentServices.GetAllDepartments();
            return View(dept);
        }
        #endregion

        #region Create Department

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatedDepartmentDTO createdDepartment)
        {
            if (!ModelState.IsValid)
                return View(createdDepartment);

            try
            {
                int result = _departmentServices.AddDepartment(createdDepartment);

                if (result > 0)
                {
                    // Department created successfully → redirect to Index
                    return RedirectToAction("Index");
                }
                else
                {
                    // Creation failed → show error
                    ModelState.AddModelError(string.Empty, $"Department {createdDepartment.Name} could not be created.");
                    return View(createdDepartment);
                }
            }
            catch (Exception ex)
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "Error creating department");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }

                return View(createdDepartment);
            }
        }


        #endregion

        #region Details of Department

        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var deptDto = _departmentServices.GetDepartmentById(id.Value);
            if (deptDto == null) return NotFound();

            // هات الموظفين في القسم ده
            var employees = _employeeRepo.GetAll()
                                         .Where(e => e.DepartmentID == id.Value)
                                         .ToList();

            // ابني الـ ViewModel النهائي
            var viewModel = new DepartmentDetailsViewModel
            {
                Id = deptDto.Id,
                Code = deptDto.Code,
                Name = deptDto.Name,
                Description = deptDto.Description,
                CreatedOn = deptDto.CreatedOn.HasValue
                    ? deptDto.CreatedOn.Value.ToDateTime(TimeOnly.MinValue)
                    : DateTime.MinValue,
                EmployeeCount = deptDto.EmployeeCount,
                TotalSalaries = employees.Sum(e => e.Salary),
                Employees = employees.Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Salary = e.Salary
                }).ToList()
            };

            return View(viewModel);
        }



        #endregion

        #region Edit Department

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var dept = _departmentServices.GetDepartmentById(id.Value);
            if (dept is null) return NotFound();
            var departmentViewModel = new DepartmentEditViewModel
            {
                Code = dept.Code,
                Name = dept.Name,
                Description = dept.Description,
                DateOfCreation = (DateOnly)dept.CreatedOn!
            };
            return View(departmentViewModel);
        }

        public IActionResult Edit([FromRoute] int id, DepartmentEditViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            try
            {
                var UpdatedDepartment = new UpdatedDepartmentDTO()
                {
                    ID = id,
                    code = viewModel.Code,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    CreatedDate = viewModel.DateOfCreation
                };

                int result = _departmentServices.UpdateDepartment(UpdatedDepartment);
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Department not updated");
                    return View(viewModel);

                }

            }
            catch (Exception EX)
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                    return View(viewModel);
                }
                else
                {
                    _logger.LogError(EX.Message);
                    return View("ErrorView", EX);
                }
            }

        }


        #endregion

        #region Delete

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _departmentServices.GetDepartmentById(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool result = _departmentServices.DeleteDepartment(id);
            if (result)
                return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError(string.Empty, "Department not deleted");
                return View();
            }
        }
            #endregion

    }
}
