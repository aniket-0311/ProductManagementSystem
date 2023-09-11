namespace ProductManagementSystem_Assign2.Models.ViewModel
{
    public class EditProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
