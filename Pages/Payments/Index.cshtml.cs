using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.Payments
{
    public class IndexModel : PageModel
    {
        private readonly Ecommerce.Data.EcommerceContext _context;

        public IndexModel(Ecommerce.Data.EcommerceContext context)
        {
            _context = context;
        }

        public IList<Payment> Payment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Payment = await _context.Payment
                .Include(p => p.Order).ToListAsync();
        }
    }
}
