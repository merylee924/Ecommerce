using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.ShoppingCarts
{
    public class EditModel : PageModel
    {
        private readonly Ecommerce.Data.EcommerceContext _context;

        public EditModel(Ecommerce.Data.EcommerceContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShoppingCart ShoppingCart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingcart =  await _context.ShoppingCart.FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingcart == null)
            {
                return NotFound();
            }
            ShoppingCart = shoppingcart;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ShoppingCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartExists(ShoppingCart.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.Id == id);
        }
    }
}
