using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductManagementSystem_Assign2.Controllers;
using ProductManagementSystem_Assign2.Data;
using ProductManagementSystem_Assign2.Data.Repositories;
using ProductManagementSystem_Assign2.Models.DomainModel;
using ProductManagementSystem_Assign2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementTest
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task List_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new List<ProductModel>
            {
                new ProductModel { Id = Guid.NewGuid(), Name = "Product 1", Category = "Category 1", Price = 58999, ImageUrl = "testUrl.com" },
                new ProductModel { Id = Guid.NewGuid(), Name = "Product 2", Category = "Category 2", Price = 32999, ImageUrl = "testUrl1.com" }
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.List();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductModel>>(viewResult.Model);
            Assert.Equal(products.Count, model.Count());
        }



        // ADD TESTS

        [Fact]
        public void Add_Get_Action_Should_Return_ViewResult()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = controller.Add();

            // Assert
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task Add_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();

            var controller = new ProductController(mockRepository.Object);
            var addProductRequest = new AddProductRequest
            {
                Name = "Test Product",
                Category = "Test Category",
                Price = 1000,
                ImageUrl = "testUrl.com"
            };

            // Act
            var result = await controller.Add(addProductRequest) as RedirectToActionResult; ;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("List", result.ActionName);
            //Assert.Equal("success", controller.TempData["NotificationType"]);
            //Assert.Equal("Product Added successfully!", controller.TempData["Notification"]);

        }

        [Fact]
        public async Task Add_InvalidModel_ReturnsViewWithTempData()
        {
            // Arrange
            var addProductRequest = new AddProductRequest
            {
                // Invalid model, missing required fields
            };

            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductController(mockRepository.Object);
            controller.ModelState.AddModelError("Name", "Name is required"); // Simulate model validation error

            // Act
            var result = await controller.Add(addProductRequest) as ViewResult;

            // Assert
            Assert.NotNull(result);
            //Assert.Equal("Product Not Added!", controller.TempData["Notification"]);
            //Assert.Equal("failure", controller.TempData["NotificationType"]);
        }



        // EDIT TESTS

        [Fact]
        public async Task Edit_Get_Action_Should_Return_ViewResult_With_Valid_Id()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productId = Guid.NewGuid(); 
            var product = new ProductModel
            {
                Id = productId,
                Name = "TestProduct",
                Category = "TestCategory",
                Price = 1099,
                ImageUrl = "test.jpg"
            };

            productRepositoryMock.Setup(repo => repo.GetById(productId))
                .ReturnsAsync(product);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.Edit(productId);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditProductRequest>(viewResult.Model);
            Assert.Equal(productId, model.Id);
           
        }

        [Fact]
        public async Task Edit_ValidModel_RedirectsToList()
        {
            // Arrange
            var editProductRequest = new EditProductRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product Name",
                Category = "Updated Category",
                Price = 2099,
                ImageUrl = "https://example.com/updated-image.jpg"
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProductModel>()))
                          .ReturnsAsync(new ProductModel()); // Mock repository method

            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.Edit(editProductRequest) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("List", result.ActionName);
            //Assert.Equal("Product Updated successfully!", controller.TempData["Notification"]);
            //Assert.Equal("success", controller.TempData["NotificationType"]);
        }


        //[Fact]
        //public async Task Edit_InvalidModel_ReturnsViewWithTempData()
        //{
        //    // Arrange
        //    var editProductRequest = new EditProductRequest
        //    {
        //        // Invalid model, missing required fields
        //    };

        //    var mockRepository = new Mock<IProductRepository>();
        //    var controller = new ProductController(mockRepository.Object);
        //    controller.ModelState.AddModelError("Name", "Name is required"); // Simulate model validation error

        //    // Act
        //    var result = await controller.Edit(editProductRequest) as ViewResult;

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("Product Not Updated!", controller.TempData["Notification"]);
        //    Assert.Equal("failure", controller.TempData["NotificationType"]);
        //}


        // DELETE TESTS

        [Fact]
        public async Task Delete_ExistingProduct_RedirectsToList()
        {
            // Arrange
            var productId = Guid.NewGuid(); // Assuming a valid product ID
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.DeleteAsync(productId))
                          .ReturnsAsync(new ProductModel()); // Mock repository method

            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.Delete(productId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("List", result.ActionName);
        }

        [Fact]
        public async Task Delete_NonExistingProduct_RedirectsToEdit()
        {
            // Arrange
            var productId = Guid.NewGuid(); // Assuming a non-existing product ID
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.DeleteAsync(productId))
                          .ReturnsAsync((ProductModel)null); // Mock repository method

            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.Delete(productId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Edit", result.ActionName);
            Assert.Equal(productId, result.RouteValues["id"]);
        }

    }
}