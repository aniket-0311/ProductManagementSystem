using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem_Assign2.Controllers;
using ProductManagementSystem_Assign2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementTest
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index_Action_Should_Return_ViewResult()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_Action_Should_Return_ViewResult()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

    }
}
