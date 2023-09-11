using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagementSystem_Assign2.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductDbContext _context;

        public UserRepository(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<IdentityUser>> GetAll(Guid id)
        {
           var users = await _context.Users.ToListAsync();
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
            var superAdminUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@gmail.com");
            
            if (superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }

            if (currentUser != null)
            {
                users.Remove(currentUser);
            }

            return users;
        }
    }
}
