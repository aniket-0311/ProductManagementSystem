using System;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem_Assign2.Controllers;
using ProductManagementSystem_Assign2.Models.ViewModel;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProductManagementTest
{
    public class AccountControllerTest
    {

        private Mock<UserManager<IdentityUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }



        private Mock<SignInManager<IdentityUser>> MockSignInManager()
        {
            var userManagerMock = MockUserManager<IdentityUser>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var logger = new Mock<ILogger<SignInManager<IdentityUser>>>();

            return new Mock<SignInManager<IdentityUser>>(
                userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                options.Object,
                logger.Object,
                null,
                null);
        }


        [Fact]
        public void Register_Get_Action_Should_Return_ViewResult()
        {
            // Arrange
            var userManagerMock = MockUserManager<IdentityUser>();
            var signInManagerMock = MockSignInManager();
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_Post_Action_Should_Return_ViewResult_When_ModelState_Invalid()
        {
            // Arrange
            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var signInManagerMock = MockSignInManager();
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);

            var registerViewModel = new RegisterViewModel();
            controller.ModelState.AddModelError("ErrorKey", "ModelError");

            // Act
            var result = await controller.Register(registerViewModel);

            // Assert
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void Login_Get_Action_Should_Return_ViewResult()
        {
            // Arrange
            var userManagerMock = MockUserManager<IdentityUser>();
            var signInManagerMock = MockSignInManager();
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = controller.Login("someReturnUrl");

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult.Model);
            Assert.IsType<LoginViewModel>(viewResult.Model);
            var model = viewResult.Model as LoginViewModel;
            Assert.Equal("someReturnUrl", model.ReturnUrl);
        }


        [Fact]
        public async Task Logout_Returns_RedirectToActionResult()
        {
            // Arrange
            var signInManagerMock = MockSignInManager();
            var controller = new AccountController(null, signInManagerMock.Object);

            // Act
            var result = await controller.Logout();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);

            // Verify that SignOutAsync was called
            signInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
        }


        [Fact]
        public async Task Login_ValidCredentialsWithReturnUrl_RedirectsToReturnUrl()
        {
            // Arrange
            var signInManagerMock = MockSignInManager();
            signInManagerMock.Setup(s => s.PasswordSignInAsync("validUser", "validPassword", false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success); // Fully qualify SignInResult

            var controller = new AccountController(null, signInManagerMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Username = "validUser",
                Password = "validPassword",
                ReturnUrl = "/somepage"
            };

            // Act
            var result = await controller.Login(loginViewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/somepage", redirectToActionResult.Url);
        }


        [Fact]
        public void AccessDenied_Returns_ViewResult()
        {
            // Arrange
            var controller = new AccountController(null, null);

            // Act
            var result = controller.AccessDenied();

            // Assert
            Assert.IsType<ViewResult>(result);
        }


    }
}
