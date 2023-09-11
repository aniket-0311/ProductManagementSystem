using Microsoft.AspNetCore.Identity;

namespace ProductManagementSystem_Assign2.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll(Guid id);
    }
}
