namespace ProductManagementSystem_Assign2.Data.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
