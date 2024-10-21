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

namespace Ecommerce.Pages.OrderItems
{
    public class EditModel : PageModel
    {
        private readonly Ecommerce.Data.EcommerceContext _context;

        public EditModel(Ecommerce.Data.EcommerceContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderItem OrderItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderitem =  await _context.OrderItem.FirstOrDefaultAsync(m => m.Id == id);
            if (orderitem == null)
            {
                return NotFound();
            }
            OrderItem = orderitem;
           ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id");
           ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description");
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

            _context.Attach(OrderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(OrderItem.Id))
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

        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.Id == id);
        }
    }
}
