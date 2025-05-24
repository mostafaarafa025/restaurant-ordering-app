using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Areas.Admin.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]

        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
