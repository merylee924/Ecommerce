using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.CartItems
{
    public class IndexModel : PageModel
    {
        private readonly Ecommerce.Data.EcommerceContext _context;

        public IndexModel(Ecommerce.Data.EcommerceContext context)
        {
            _context = context;
        }

        public IList<CartItem> CartItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CartItem = await _context.CartItem
                .Include(c => c.Cart)
                .Include(c => c.Product).ToListAsync();
        }
    }
}
