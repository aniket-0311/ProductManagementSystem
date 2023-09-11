using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ProductManagementSystem_Assign2.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly Account account;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            account = new Account(
                _configuration.GetSection("Cloudinary")["CloudinaryName"],
                 _configuration.GetSection("Cloudinary")["ApiKey"],
                  _configuration.GetSection("Cloudinary")["ApiSecret"]);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };

            var uploadResult = await client.UploadAsync(uploadParams);
            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
    }
}
