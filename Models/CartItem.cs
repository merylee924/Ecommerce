namespace Ecommerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // Foreign key for the ShoppingCart
        public int CartId { get; set; }

        // Foreign key for the Product
        public int ProductId { get; set; }

        // Quantity of the product being added to the cart
        public int Quantity { get; set; }

        // Navigation property for the ShoppingCart
        public ShoppingCart Cart { get; set; }

        // Navigation property for the Product
        public Product Product { get; set; }

        // Calculate the subtotal for this cart item (Price * Quantity)
        // Ensure Product is not null before accessing its properties
        public decimal Subtotal => (Product != null ? Product.Price : 0) * Quantity;
    }
}
