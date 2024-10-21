using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;

namespace Ecommerce.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<CartItem> CartItem { get; set; } = default!;
        public DbSet<OrderItem> OrderItem { get; set; } = default!;
        public DbSet<Payment> Payment { get; set; } = default!;
        public DbSet<ShoppingCart> ShoppingCart { get; set; } = default!;
    }
}
