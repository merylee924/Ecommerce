using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Pages.OrderItems
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
        ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public OrderItem OrderItem { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OrderItem.Add(OrderItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
