using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ecommerce.Models;
using Ecommerce.Services;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisProductController : ControllerBase
    {
        private readonly RedisService _redisService;

        public RedisProductController(RedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost("populate")]
        public IActionResult PopulateRedis()
        {
            try
            {
                var products = new List<Product>
                {
                    new Product { Id = 1, Name = "Smartphone", Description = "Un smartphone haut de gamme.", Price = 699.99m, ImageURL = "images/smartphone.jpeg", CategoryId = 1 },
                    new Product { Id = 2, Name = "Laptop", Description = "Un ordinateur portable puissant.", Price = 1009.99m, ImageURL = "images/laptop.jpg", CategoryId = 1 },
                    new Product { Id = 3, Name = "T-shirt", Description = "Un t-shirt en coton.", Price = 19.99m, ImageURL = "images/tshirt.jpg", CategoryId = 2 }
                };

                foreach (var product in products)
                {
                    var serializedProduct = JsonConvert.SerializeObject(product);
                    _redisService.Database.StringSet($"product:{product.Id}", serializedProduct);
                }

                return Ok(new { message = "Les produits ont été ajoutés avec succès dans Redis." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la sauvegarde dans Redis.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                var redisValue = _redisService.Database.StringGet($"product:{id}");
                if (redisValue.IsNullOrEmpty)
                    return NotFound(new { message = "Produit non trouvé dans Redis." });

                var product = JsonConvert.DeserializeObject<Product>(redisValue);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération depuis Redis.", error = ex.Message });
            }
        }
    }
}
