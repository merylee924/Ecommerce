using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Tp_Panier.Models;

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
        public DbSet<User> Users { get; set; } = default!;

        // Nouvelles entités
        public DbSet<Store> Stores { get; set; } = default!;
        public DbSet<StoreProduct> StoreProducts { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurer la relation many-to-many entre Store et Product
            modelBuilder.Entity<StoreProduct>()
                .HasKey(sp => sp.Id);

            modelBuilder.Entity<StoreProduct>()
                .HasOne(sp => sp.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(sp => sp.StoreId);

            modelBuilder.Entity<StoreProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.Stores)
                .HasForeignKey(sp => sp.ProductId);
        }
    }
}
