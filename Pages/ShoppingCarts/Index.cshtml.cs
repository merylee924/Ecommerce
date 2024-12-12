using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Pages.ShoppingCarts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EcommerceContext _context;

        public IndexModel(EcommerceContext context)
        {
            _context = context;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Nouvelle propriété pour afficher le nombre d'articles dans le panier
        public int CartItemCount { get; set; }

        public void OnGet()
        {

            
            // Charger les articles du panier depuis les cookies ou la base de données
            CartItems = Request.Cookies.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Compter le nombre d'articles dans le panier
            CartItemCount = CartItems.Sum(item => item.Quantity);

            // Si tu utilises une base de données pour stocker le panier
            /*
            var userId = ... // Obtenir l'ID de l'utilisateur actuel ou l'identifiant de la session
            CartItems = _context.CartItems
                                .Include(c => c.Product) // Chargement anticipé des données du produit
                                .Where(c => c.Cart.UserId == userId) // Filtrer par l'utilisateur actuel
                                .ToList();
            CartItemCount = CartItems.Sum(item => item.Quantity);
            */
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int productId)
        {
            // Récupérer le panier à partir des cookies
            List<CartItem> cart = Request.Cookies.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Trouver l'élément à supprimer
            var itemToRemove = cart.FirstOrDefault(ci => ci.ProductId == productId);

            if (itemToRemove != null)
            {
                // Supprimer l'élément du panier
                cart.Remove(itemToRemove);

                // Mettre à jour le panier dans les cookies
                Response.Cookies.SetObject("Cart", cart, 30 * 24 * 60); // 30 jours en minutes

                // Message de succès
                TempData["SuccessMessage"] = "Item removed from cart successfully.";
            }
            else
            {
                // Message d'erreur si l'élément n'existe pas dans le panier
                TempData["ErrorMessage"] = "Item not found in the cart.";
            }

            return RedirectToPage();
        }


    }
}
