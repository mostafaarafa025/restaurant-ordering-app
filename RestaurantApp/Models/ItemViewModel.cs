using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        
        public IFormFile ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int Calories { get; set; }
        [Required]
        public List<string> Ingredients { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
