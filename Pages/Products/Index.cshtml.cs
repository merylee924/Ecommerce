using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly EcommerceContext _context;

        public IndexModel(EcommerceContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; }
        public IList<Category> Categories { get; set; }
        public string SearchTerm { get; set; }
        public int? SelectedCategoryId { get; set; }

        public async Task OnGetAsync(string searchTerm, int? categoryId)
        {
            SearchTerm = searchTerm;
            SelectedCategoryId = categoryId;

            Categories = await _context.Category.ToListAsync();

            IQueryable<Product> query = _context.Product;

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(SearchTerm) || p.Description.Contains(SearchTerm));
            }

            if (SelectedCategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == SelectedCategoryId);
            }

            Products = await query.ToListAsync();
        }

        // Add to cart method using cookies
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var product = await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return NotFound();

            // Retrieve the cart from the cookie, or create a new one if it doesn't exist
            List<CartItem> cart = Request.Cookies.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Check if the product is already in the cart
            var cartItem = cart.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++; // Increment quantity if the item is already in the cart
            }
            else
            {
                // Add a new item to the cart
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    Product = product
                });
            }

            // Save the updated cart in the cookie
            Response.Cookies.SetObject("Cart", cart, 30 * 24 * 60); // 30 days in minutes

            // Set success message in TempData
            TempData["SuccessMessage"] = $"{product.Name} added to cart successfully.";

            return RedirectToPage();
        }

    }
}
