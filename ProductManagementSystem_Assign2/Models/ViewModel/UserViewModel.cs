namespace ProductManagementSystem_Assign2.Models.ViewModel
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }
        public Dictionary<string, List<string>> UserRoles { get; set; } = new Dictionary<string, List<string>>();
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
        public bool AdminRoleCheckbox { get; set; }
    }
}
