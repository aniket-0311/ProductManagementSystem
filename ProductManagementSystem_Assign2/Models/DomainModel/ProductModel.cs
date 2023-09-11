using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem_Assign2.Models.DomainModel
{
    public class ProductModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
