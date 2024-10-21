using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Ecommerce.Data; // Ensure you have the correct namespace for your DbContext

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context
            builder.Services.AddDbContext<EcommerceContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceContext")
                ?? throw new InvalidOperationException("Connection string 'EcommerceContext' not found.")));

            // Add Razor Pages and configure JSON options to handle circular references
            builder.Services.AddRazorPages()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Remove authentication and authorization
            // app.UseAuthentication(); // Removed
            // app.UseAuthorization();  // Removed

            app.MapRazorPages();

            app.Run();
        }
    }
}
