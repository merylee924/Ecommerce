using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est requis.")]
        [StringLength(100, ErrorMessage = "Le nom de la catégorie ne doit pas dépasser 100 caractères.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La description est requise.")]
        [StringLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string Description { get; set; }

        public string IconURL { get; set; } // Ajout de la propriété pour l'icône

        // [JsonIgnore] to prevent circular reference during serialization
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
