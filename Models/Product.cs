using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        [Url(ErrorMessage = "Please provide a valid URL for the image.")]
        public string ImageURL { get; set; }

        // Foreign key for Category
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        // Navigation property for Category
        public virtual Category Category { get; set; }

        // Initializing collections to avoid null reference exceptions
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
