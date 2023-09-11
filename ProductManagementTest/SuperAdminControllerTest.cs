using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagementSystem_Assign2.Controllers;
using ProductManagementSystem_Assign2.Data.Repositories;
using ProductManagementSystem_Assign2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementTest
{
    public class SuperAdminControllerTest
    {
        [Fact]
        public async Task List_Get_Action_Should_Return_ViewResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);

            // Simulate an authenticated user with the "Admin" role
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "admin@example.com"),
                new Claim(ClaimTypes.Role, "Admin") // Simulate "Admin" role
            }, "mock"));

            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser()); // Simulate a user

            var superAdminController = new SuperAdminController(userRepositoryMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            // Act
            var result = await superAdminController.List();

            // Assert
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task List_Post_Action_Should_Return_RedirectToAction_When_User_Creation_Succeeds()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);

            // Simulate an authenticated user with the "Admin" role
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "admin@example.com"),
                new Claim(ClaimTypes.Role, "Admin") // Simulate "Admin" role
            }, "mock"));

            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser()); // Simulate a user

            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(um => um.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            var superAdminController = new SuperAdminController(userRepositoryMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            var userModel = new UserViewModel
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "password",
                AdminRoleCheckbox = true // Simulate admin role selection
            };

            // Act
            var result = await superAdminController.List(userModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectToActionResult.ActionName);
            Assert.Equal("SuperAdmin", redirectToActionResult.ControllerName);
        }


        [Fact]
        public async Task Delete_Action_Should_Return_RedirectToAction_When_User_Deletion_Succeeds()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);

            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser()); // Simulate a found user

            userManagerMock.Setup(um => um.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success); // Simulate user deletion success

            // Simulate an authenticated user with the "Admin" role
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "admin@example.com"),
                new Claim(ClaimTypes.Role, "Admin") // Simulate "Admin" role
            }, "mock"));

            var superAdminController = new SuperAdminController(userRepositoryMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            var userId = Guid.NewGuid(); // Simulate a user ID

            // Act
            var result = await superAdminController.Delete(userId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectToActionResult.ActionName);
            Assert.Equal("SuperAdmin", redirectToActionResult.ControllerName);
        }

    }
}
