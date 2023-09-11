using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem_Assign2.Models.ViewModel;

namespace ProductManagementSystem_Assign2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email
                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if(identityResult.Succeeded)
                {
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                    if(roleIdentityResult.Succeeded)
                    {
                        // Show Success Notification 
                        //TempData["Notification"] = "Register successful!";
                        //TempData["NotificationType"] = "success";

                        return RedirectToAction("Login", "Account");
                    }
                }

                else
                {
                    foreach (var error in identityResult.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Username", "Username already exists.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            //Show error notification
            //TempData["Notification"] = "Register Failed!";
            //TempData["NotificationType"] = "failure";

            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
                    loginViewModel.Password, false, false);

                if (signInResult.Succeeded && !string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    // Success Notification (only when login is successful and ReturnUrl is provided)
                    //TempData["Notification"] = "Login successful!";
                    //TempData["NotificationType"] = "success";

                    return Redirect(loginViewModel.ReturnUrl);
                }
                else if (signInResult.Succeeded)
                {
                    // Success Notification (when login is successful but no ReturnUrl)
                    //TempData["Notification"] = "Login successful!";
                    //TempData["NotificationType"] = "success";

                    return RedirectToAction("Index", "Home");
                }
            }

            // Show error notification
            //TempData["Notification"] = "Invalid Username or Password!";
            //TempData["NotificationType"] = "failure";

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            //Success Notification
            //Success Notification
            //TempData["Notification"] = "Logout successful!";
            //TempData["NotificationType"] = "success";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
