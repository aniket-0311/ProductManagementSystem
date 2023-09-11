using ProductManagementSystem_Assign2.Models.DomainModel;

namespace ProductManagementSystem_Assign2.Data.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllAsync();
        Task<ProductModel?> GetById(Guid id);
        Task<ProductModel> AddAsync(ProductModel productModel);
        Task<ProductModel?> UpdateAsync(ProductModel productModel);
        Task<ProductModel?> DeleteAsync(Guid id);
    }
}
