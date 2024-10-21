namespace Ecommerce.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
