using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // [JsonIgnore] to prevent circular reference during serialization
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
