using Microsoft.EntityFrameworkCore;
using ProductManagementSystem_Assign2.Models.DomainModel;

namespace ProductManagementSystem_Assign2.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        // METHODS FOR CRUD
        public async Task<ProductModel> AddAsync(ProductModel productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<ProductModel?> DeleteAsync(Guid id)
        {
            var productToDelete = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
                return productToDelete;
            }

            return null;
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductModel?> GetById(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductModel?> UpdateAsync(ProductModel productModel)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == productModel.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = productModel.Name;
                existingProduct.Category = productModel.Category;
                existingProduct.Price = productModel.Price;
                existingProduct.ImageUrl = productModel.ImageUrl;

                await _context.SaveChangesAsync();
                return productModel;
            }

            return null;
        }
    }
}
