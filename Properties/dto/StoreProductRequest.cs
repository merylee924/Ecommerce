using Ecommerce.Models;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Properties.dto
{
    public class StoreProductRequest
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        public Product Product { get; set; }
    }

}
