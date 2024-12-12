using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public virtual ICollection<StoreProduct> Products { get; set; } = new List<StoreProduct>();
    }

}
