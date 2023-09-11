using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem_Assign2.Data.Repositories;
using ProductManagementSystem_Assign2.Models.DomainModel;
using ProductManagementSystem_Assign2.Models.ViewModel;
using System.Data;

namespace ProductManagementSystem_Assign2.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var productData = await _productRepository.GetAllAsync();
            return View(productData);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddProductRequest addProductRequest)
        {
            if (!ModelState.IsValid)
            {
                //Failure Notification
                //TempData["Notification"] = "Product Not Added!";
                //TempData["NotificationType"] = "failure";

                return View();
            }
            var domainModel = new ProductModel
            {
                Name = addProductRequest.Name,
                Category = addProductRequest.Category,
                Price = addProductRequest.Price,
                ImageUrl = addProductRequest.ImageUrl,
            };

            await _productRepository.AddAsync(domainModel);

            //Success Notification
            //TempData["Notification"] = "Product Added successfully!";
            //TempData["NotificationType"] = "success";

            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
                var product = await _productRepository.GetById(id);
                if (product != null)
                {
                    var editedProduct = new EditProductRequest
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = product.Category,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl
                    };

                    return View(editedProduct);
                }

                return View(null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditProductRequest editProductRequest)
        {
            if (!ModelState.IsValid)
            {
                //Failure Notification
                TempData["Notification"] = "Product Not Updated!";
                TempData["NotificationType"] = "failure";
                return RedirectToAction("Edit", new { id = editProductRequest.Id });
            }
                var product = new ProductModel
            {
                Id=editProductRequest.Id,
                Name=editProductRequest.Name,
                Price=editProductRequest.Price,
                Category = editProductRequest.Category,
                ImageUrl=editProductRequest.ImageUrl
            };

            var updatedProduct = await _productRepository.UpdateAsync(product);
            if(updatedProduct != null)
            {
                //Success Notification
                //TempData["Notification"] = "Product Updated successfully!";
                //TempData["NotificationType"] = "success";

                return RedirectToAction("List");
            }

            //Failure Notification
            //TempData["Notification"] = "Product Not Updated!";
            //TempData["NotificationType"] = "failure";

            return RedirectToAction("Edit", new { id = editProductRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productData = await _productRepository.DeleteAsync(id);
            if(productData != null)
            {
                //Success Notification
                //TempData["Notification"] = "Product Deleted successfully!";
                //TempData["NotificationType"] = "success";

                return RedirectToAction("List");
            }

            //Failure Notification
            //TempData["Notification"] = "Product Not Deleted!";
            //TempData["NotificationType"] = "failure";
            return RedirectToAction("Edit", new {id});
        }
    }
}
