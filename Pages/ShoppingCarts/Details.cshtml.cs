using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.ShoppingCarts
{
    public class DetailsModel : PageModel
    {
        private readonly Ecommerce.Data.EcommerceContext _context;

        public DetailsModel(Ecommerce.Data.EcommerceContext context)
        {
            _context = context;
        }

        public ShoppingCart ShoppingCart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingcart = await _context.ShoppingCart.FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingcart == null)
            {
                return NotFound();
            }
            else
            {
                ShoppingCart = shoppingcart;
            }
            return Page();
        }
    }
}
