using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }
    }
}
