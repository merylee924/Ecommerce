using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.Payments
{
    public class CreateModel : PageModel
    {
        private readonly Ecommerce.Data.EcommerceContext _context;

        public CreateModel(Ecommerce.Data.EcommerceContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Payment Payment { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Payment.Add(Payment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
