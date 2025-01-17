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

        [HttpPost("create")]
         public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest(new { message = "Le produit ne peut pas être null." });
                }

                // Validation des données du produit
                if (string.IsNullOrEmpty(product.Name))
                {
                    return BadRequest(new { message = "Le nom du produit est requis." });
                }

                if (string.IsNullOrEmpty(product.Description))
                {
                    return BadRequest(new { message = "La description du produit est requise." });
                }

                if (product.Price <= 0)
                {
                    return BadRequest(new { message = "Le prix du produit doit être supérieur à zéro." });
                }

                if (product.CategoryId <= 0)
                {
                    return BadRequest(new { message = "L'ID de la catégorie est requis." });
                }

                // Vérifier si un produit avec le même ID existe déjà
                var existingProduct = _redisService.Database.StringGet($"product:{product.Id}");
                if (!existingProduct.IsNullOrEmpty)
                {
                    return Conflict(new { message = "Un produit avec cet ID existe déjà." });
                }

                // Sérialiser et enregistrer le produit dans Redis
                var serializedProduct = JsonConvert.SerializeObject(product);
                _redisService.Database.StringSet($"product:{product.Id}", serializedProduct);

                return Ok(new { message = "Produit créé avec succès.", product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la création du produit.", error = ex.Message });
            }
        }



        [HttpGet("all")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var server = _redisService.GetServer();
                var keys = server.Keys(pattern: "product:*");
                var products = new List<Product>();

                foreach (var key in keys)
                {
                    var redisValue = _redisService.Database.StringGet(key);
                    if (!redisValue.IsNullOrEmpty)
                    {
                        var product = JsonConvert.DeserializeObject<Product>(redisValue);
                        products.Add(product);
                    }
                }

                if (products.Count == 0)
                {
                    return NotFound(new { message = "Aucun produit trouvé dans Redis." });
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération des produits.", error = ex.Message });
            }
        }
    }
}
