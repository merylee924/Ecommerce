using Ecommerce.dto;
using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly RedisService _redisService;

    public CartController(RedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpPost("add")]
    public IActionResult AddToCart([FromBody] AddToCartDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid request");

        // Retrieve product details from Redis
        var productJson = _redisService.Database.StringGet($"product:{dto.ProductId}");
        if (productJson.IsNullOrEmpty)
            return NotFound("Product not found");

        var product = JsonConvert.DeserializeObject<Product>(productJson);
        if (product == null)
            return NotFound("Product not found");

        // Check if the product is out of stock
        if (product.OutOfStock)
            return BadRequest("Product is out of stock");

        // Retrieve cart from Redis
        var cartJson = _redisService.Database.StringGet($"cart:{dto.CartId}");
        ShoppingCart cart;

        if (cartJson.IsNullOrEmpty)
        {
            cart = new ShoppingCart { Id = dto.CartId, CreatedAt = DateTime.UtcNow, CartItems = new List<CartItem>() };
        }
        else
        {
            cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
        }

        // Add the product to the cart
        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            int newQuantity = existingItem.Quantity + dto.Quantity;

            // Check if the total quantity in the cart exceeds the available quantity
            if (newQuantity > product.Quantity)
            {
                return BadRequest($"Cannot add more than {product.Quantity} units of this product to the cart.");
            }

            existingItem.Quantity = newQuantity; // Update the quantity if within limits
        }
        else
        {
            // Check if the quantity being added exceeds the available stock
            if (dto.Quantity > product.Quantity)
            {
                return BadRequest($"Cannot add more than {product.Quantity} units of this product to the cart.");
            }

            cart.CartItems.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Product = product  // Add product details here
            });
        }

        // Save the updated cart to Redis
        var serializedCart = JsonConvert.SerializeObject(cart);
        _redisService.Database.StringSet($"cart:{dto.CartId}", serializedCart);

        return Ok(new { message = "Product added to cart successfully." });
    }

    [HttpGet("{cartId}")]
    public IActionResult GetCartContents(int cartId)
    {
        // Retrieve cart from Redis using cart ID
        var cartJson = _redisService.Database.StringGet($"cart:{cartId}");

        if (cartJson.IsNullOrEmpty)
        {
            return NotFound(new { message = "Cart not found." });
        }

        // Deserialize the cart JSON string into a ShoppingCart object
        var cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);

        // Retrieve product details for each cart item
        foreach (var item in cart.CartItems)
        {
            // Retrieve product details from Redis (assuming products are stored in Redis with key "product:{productId}")
            var productJson = _redisService.Database.StringGet($"product:{item.ProductId}");
            if (!productJson.IsNullOrEmpty)
            {
                item.Product = JsonConvert.DeserializeObject<Product>(productJson);
            }
        }

        // Return the cart contents with product details
        return Ok(cart);
    }
    [HttpPost("UpdateCartItemQuantity")]
    public IActionResult UpdateCartItemQuantity([FromBody] AddToCartDto dto)
    {
        if (dto == null || dto.Quantity <= 0)
            return BadRequest("Invalid request");

        // Retrieve the cart from Redis
        var cartJson = _redisService.Database.StringGet($"cart:{dto.CartId}");
        if (cartJson.IsNullOrEmpty)
            return NotFound("Cart not found");

        var cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);

        // Find the cart item
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
        if (cartItem == null)
            return NotFound("Cart item not found");

        // Retrieve the product to check available stock
        var productJson = _redisService.Database.StringGet($"product:{dto.ProductId}");
        if (productJson.IsNullOrEmpty)
            return NotFound("Product not found");

        var product = JsonConvert.DeserializeObject<Product>(productJson);

        // Check stock
        if (dto.Quantity > product.Quantity)
            return BadRequest($"Cannot exceed {product.Quantity} units in stock.");

        // Update quantity
        cartItem.Quantity = dto.Quantity;

        // Save updated cart back to Redis
        var serializedCart = JsonConvert.SerializeObject(cart);
        _redisService.Database.StringSet($"cart:{dto.CartId}", serializedCart);

        return Ok(new { message = "Cart item quantity updated successfully." });
    }




}
