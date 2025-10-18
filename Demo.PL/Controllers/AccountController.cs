using Demo.DAL.Models.IDentityModel;
using Demo.PL.Utilities;
using Demo.PL.ViewModels;
using Demo.PL.Views.Account;
using Demo.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Demo.PL.Controllers
{
    public class AccountController(UserManager<Application_User> _userManager , SignInManager<Application_User> signInManager) : Controller
    {

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            // Return the registration view
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = new Application_User
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
            };

            var Result = _userManager.CreateAsync(user, viewModel.Password).Result;
            if (Result.Succeeded)
            {
                // User created successfully
                return RedirectToAction("Login");
            }
            else
            {
                // Handle errors
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            // Return the login view
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
            if (user != null)
            {
                var result = _userManager.CheckPasswordAsync(user, viewModel.Password).Result;
                if (result)
                {
                    // Login successful
                    return RedirectToAction("Index", "Home");
                }
            }
            // Handle login failure
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(viewModel);
        }

        public IActionResult LoginWithGoogle()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"),
            };
            return Challenge(prop,GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claim = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new 
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Simulate logout logic
            // Logout successful
            return Ok("Logout successful.");
        }
        #endregion

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        #endregion

        #region SendEmailForResetingPassword

        public IActionResult SendResetPasswordLink(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(forgetPasswordViewModel.Email).Result;
                if (user is not null)
                {
                    var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = forgetPasswordViewModel.Email, token }, Request.Scheme);
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
            }
                ModelState.AddModelError(string.Empty, "Invalid Operation.");
                return View(nameof(ForgetPassword),forgetPasswordViewModel);
        }

        #endregion

        #region CheckYourInbox
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region ResetPassword

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }

            string email = TempData["email"] as string ?? string.Empty;
            string token = TempData["token"] as string ?? string.Empty;

            var user = _userManager.FindByEmailAsync(email).Result;
            if (user is not null)
            {
                var Result = _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword).Result;
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                   foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
                return View( nameof(ResetPassword), resetPasswordViewModel);
        }
        #endregion

        #region SignOut

        [HttpGet]
        public new IActionResult SignOut()
        {
            signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }   

        #endregion
    }
}
