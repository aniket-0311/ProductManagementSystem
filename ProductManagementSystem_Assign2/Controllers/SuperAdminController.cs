using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem_Assign2.Data.Repositories;
using ProductManagementSystem_Assign2.Models.ViewModel;
using System.Data;
using System.Net.Mail;

namespace ProductManagementSystem_Assign2.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SuperAdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public SuperAdminController(
                IUserRepository userRepository, 
                UserManager<IdentityUser> userManager
                )
        {
            _userRepository = userRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var usersList = await _userRepository.GetAll(Guid.Parse(currentUser.Id));

            var userViewModel = new UserViewModel
            {
                Users = new List<User>()
            };

            foreach (var user in usersList)
            {
                var identityUser = await userManager.FindByIdAsync(user.Id);

                userViewModel.Users.Add(new Models.ViewModel.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });

                var roles = await userManager.GetRolesAsync(identityUser);

                var primaryRole = roles.Contains("Admin") ? "Admin" : "User";

                userViewModel.UserRoles[identityUser.Id] = new List<string> { primaryRole };
            }
            return View(userViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult != null)
            {
                if (identityResult.Succeeded)
                {

                    // assign roles to this user
                    var roles = new List<string> { "User" };

                    if (request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }

                    identityResult =
                        await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult != null && identityResult.Succeeded)
                    {

                        //Success Notification
                        //TempData["Notification"] = "User Added successfully!";
                        //TempData["NotificationType"] = "success";

                        return RedirectToAction("List", "SuperAdmin");
                    }
                }
            }

            //Failure Notification
            //TempData["Notification"] = "User Not Added!";
            //TempData["NotificationType"] = "failure";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult != null && identityResult.Succeeded)
                {

                    //Success Notification
                    //TempData["Notification"] = "User Deleted successfully!";
                    //TempData["NotificationType"] = "success";


                    return RedirectToAction("List", "SuperAdmin");
                }
            }

            //Success Notification
            //TempData["Notification"] = "User Not Deleted!";
            //TempData["NotificationType"] = "failure";

            return View();
        }
    }
}
