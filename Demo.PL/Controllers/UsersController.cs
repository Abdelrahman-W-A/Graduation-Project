using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;
using Demo.BLL.Services.EmployeeServices;
using Demo.BLL.Services.Model_Services.UserServices;
using Demo.DAL.Models.IDentityModel;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class UsersController(UserManager<Application_User> _userManager , IUsersServices _users, IWebHostEnvironment environment, ILogger<UsersController> logger) : Controller
    {

        #region Index
        [HttpGet]
        public IActionResult Index(string UserSearchName)
        {
            var users = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(UserSearchName))
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(UserSearchName.ToLower()));
            }
            var userList = users.Select(u => new CreatedUserDTO
            {
                Id = u.Id,
                FName = u.FirstName,
                LName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
            }).ToList();

            return View(userList);
        }

        #endregion

        #region Details

        [HttpGet]
        public IActionResult Details([FromRoute] string? id)
        {
            if (id is null) return BadRequest($"Invalid User ID : {id}");
            var user = _users.GetUserById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        #endregion

        #region Delete

        [HttpGet]
        public IActionResult Delete(string? id)
        {
            if (id is null) return BadRequest();
            var user = _users.GetUserById(id);
            if (user is null) return NotFound();

            var employeeViewModel = new CreatedUserDTO()
            {
                Id = user.Id,
                FName = user.FName,
                LName = user.LName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,

            };
            return View(employeeViewModel);
        }

        [HttpPost]
        public IActionResult Delete(string? id , CreatedUserDTO createdUserDTO)
        {
            if (id is null) return BadRequest();
            try
            {
                var deleted = _users.DeleteUser(id);
                if (deleted) return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "User is not deleted");
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

        #region Update

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string? id)
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            return View(new UsersUpdatedDTO()
            {
                FName = user.FirstName,
                LName = user.LastName,
                PhoneNumber = user.PhoneNumber
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, UsersUpdatedDTO updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }
            var meassge = string.Empty;

            try
            {
                var user = await _userManager.FindByIdAsync(id); // Fetch the Application_User object
                if (user is null)
                {
                    return NotFound();
                }

                // Update the Application_User object with the new values
                user.FirstName = updatedUser.FName;
                user.LastName = updatedUser.LName;
                user.PhoneNumber = updatedUser.PhoneNumber;
                var result = await _userManager.UpdateAsync(user); // Pass the correct type
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    meassge = "User can not be updated";
                }
            }
            catch (Exception ex)
            {
                meassge = environment.IsDevelopment() ? ex.Message : "User Can not ne updated";
            }
            return View(updatedUser);
        }

        #endregion

    }
}
