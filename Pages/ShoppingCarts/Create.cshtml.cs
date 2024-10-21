using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.ShoppingCarts
{
    public class CreateModel : PageModel
    {
        private readonly EcommerceContext _context;

        public CreateModel(EcommerceContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Si vous n'avez pas besoin d'associer un utilisateur, vous pouvez supprimer cette ligne
            // ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ShoppingCart ShoppingCart { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ajout du panier à la base de données
            _context.ShoppingCart.Add(ShoppingCart);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
