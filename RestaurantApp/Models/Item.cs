using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImagePath {  get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int Calories {  get; set; }
        [Required]
        public List<string> Ingredients { get; set; }

        [ForeignKey("Category")]
        [Required]
        public int CategoryId {  get; set; }
        public virtual Category? Category { get; set; }

    }
}
