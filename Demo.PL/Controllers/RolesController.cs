using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Data_Transfer_Objects__DTOs_.RolesDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.UserDTOs;
using Demo.BLL.Services.Model_Services.RolesServices;
using Demo.BLL.Services.Model_Services.UserServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class RolesController(RoleManager<IdentityRole> roleManager ,IRolesServices _users, IWebHostEnvironment environment, ILogger<UsersController> logger) : Controller
    {
        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string RoleSearchName)
        {
            var users = roleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(RoleSearchName))
            {
                users = users.Where(u => u.Name.ToLower().Contains(RoleSearchName.ToLower()));
            }
            var userList = await users.Select(u => new CreatedRoleDTO
            {
                Id = u.Id,
                RoleName = u.Name
            }).ToListAsync();

            return View(userList);
        }

        #endregion

        #region Details

        [HttpGet]
        public IActionResult Details([FromRoute] string? id)
        {
            if (id is null) return BadRequest($"Invalid User ID : {id}");
            var user = _users.GetRoleById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        #endregion

        #region Delete

        [HttpGet]
        public IActionResult Delete(string? id)
        {
            if (id is null) return BadRequest();
            var user = _users.GetRoleById(id);
            if (user is null) return NotFound();

            var employeeViewModel = new CreatedRoleDTO()
            {
                Id = user.Id,
                RoleName = user.RoleName

            };
            return View(employeeViewModel);
        }

        [HttpPost]
        public IActionResult Delete(string? id, CreatedRoleDTO createdUserDTO)
        {
            if (id is null) return BadRequest();
            try
            {
                var deleted = _users.DeleteRole(id);
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
            var user = await roleManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            return View(new RoleEditDTO()
            {
                Id = user.Id,
                RoleName = user.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleEditDTO updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }
            var meassge = string.Empty;

            try
            {
                var user = await roleManager.FindByIdAsync(id); // Fetch the Application_User object
                if (user is null)
                {
                    return NotFound();
                }

                // Update the Application_User object with the new values
                user.Id = updatedUser.Id;
                user.Name = updatedUser.RoleName;

                var result = await roleManager.UpdateAsync(user); // Pass the correct type
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

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatedRoleDTO createdRoleDTO)
        {
            if (ModelState.IsValid) // server side validation
            {
                roleManager.CreateAsync(new IdentityRole()
               { 
                    Name = createdRoleDTO.RoleName
                });

                return RedirectToAction(nameof(Index));
            }

            return View(createdRoleDTO);
        }
        #endregion
    }
}
