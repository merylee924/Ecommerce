using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Services;
using Newtonsoft.Json;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly RedisService _redisService;

        public CategoryController(RedisService redisService)
        {
            _redisService = redisService;
        }

        // Ajouter plusieurs catégories
        [HttpPost("add")]
        public IActionResult AddCategories([FromBody] List<Category> categories)
        {
            try
            {
                if (categories == null || categories.Count == 0)
                {
                    return BadRequest(new { message = "La liste des catégories est vide ou invalide." });
                }

                foreach (var category in categories)
                {
                    // Vérifier si le nom de l'icône est non vide
                    if (string.IsNullOrEmpty(category.IconURL))
                    {
                        return BadRequest(new { message = $"Le nom de l'icône pour la catégorie '{category.Name}' est requis." });
                    }

                    // Vous pouvez ajouter des vérifications supplémentaires ici si nécessaire (par exemple, vérifier que le nom de fichier correspond à un certain format).
                    // Par exemple : si (category.IconURL.Contains(".png") || category.IconURL.Contains(".jpg"))

                    var serializedCategory = JsonConvert.SerializeObject(category);
                    _redisService.Database.StringSet($"category:{category.Id}", serializedCategory);
                }

                return Ok(new { message = "Les catégories ont été ajoutées avec succès." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de l'ajout des catégories.", error = ex.Message });
            }
        }

        // Récupérer toutes les catégories
        [HttpGet("all")]
        public IActionResult GetAllCategories()
        {
            try
            {
                var server = _redisService.GetServer();
                var keys = server.Keys(pattern: "category:*");
                var categories = new List<Category>();

                foreach (var key in keys)
                {
                    var redisValue = _redisService.Database.StringGet(key);
                    if (!redisValue.IsNullOrEmpty)
                    {
                        var category = JsonConvert.DeserializeObject<Category>(redisValue);
                        categories.Add(category);
                    }
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération des catégories.", error = ex.Message });
            }
        }

        [HttpGet("{categoryId}/products")]
        public IActionResult GetProductsByCategoryId(int categoryId)
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
                        if (product.CategoryId == categoryId)
                        {
                            products.Add(product);
                        }
                    }
                }

                if (products.Count == 0)
                {
                    return NotFound(new { message = $"Aucun produit trouvé pour la catégorie ID {categoryId}." });
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
