using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly EcommerceContext _context;

        public DetailsModel(EcommerceContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        // Ajout de la méthode pour ajouter un produit au panier
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            // Rechercher le produit à ajouter au panier
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Chercher un panier existant ou en créer un nouveau si aucun n'existe
            var cart = await _context.ShoppingCart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == 1); // Remplacer par une logique pour récupérer le panier sans UserId

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    CreatedAt = DateTime.UtcNow // Si non lié à un utilisateur
                };
                _context.ShoppingCart.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Vérifier si le produit est déjà dans le panier
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // Si le produit est déjà dans le panier, augmenter la quantité
                existingCartItem.Quantity++;
            }
            else
            {
                // Sinon, créer un nouvel article de panier
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                _context.CartItem.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Rediriger vers la page du panier
            return RedirectToPage("/ShoppingCart");
        }
    }
}
