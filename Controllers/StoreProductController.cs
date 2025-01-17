using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ecommerce.Models;
using Ecommerce.Services;
using Ecommerce.Properties.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using Ecommerce.dto;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreProductController : ControllerBase
    {
        private readonly RedisService _redisService;

        public StoreProductController(RedisService redisService)
        {
            _redisService = redisService;
        }

        // Endpoint to create a store (already exists)
        [HttpPost("add")]
        public IActionResult CreateStore([FromBody] StoreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Serialize the store data and save it in Redis using a key pattern "store:{storeId}"
                var serializedStore = JsonConvert.SerializeObject(request);
                _redisService.Database.StringSet($"store:{request.Id}", serializedStore);

                return Ok(new { message = "Magasin créé avec succès dans Redis." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la création du magasin dans Redis.", error = ex.Message });
            }
        }

        // Endpoint to add a product to a store
        [HttpPost("{storeId}/addProduct")]
        public IActionResult AddProductToStore(int storeId, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var db = _redisService.Database;
                var serializedProduct = JsonConvert.SerializeObject(product);

                // Store product in Redis list under the key "store:{storeId}:products"
                db.ListRightPush($"store:{storeId}:products", serializedProduct);

                return Ok(new { message = "Produit ajouté avec succès au magasin." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de l'ajout du produit au magasin.", error = ex.Message });
            }
        }

        // Endpoint to get products by store
        [HttpGet("{storeId}/products")]
        public IActionResult GetProductsByStore(int storeId)
        {
            try
            {
                var db = _redisService.Database;
                var productKeys = db.ListRange($"store:{storeId}:products");

                var products = productKeys
                    .Select(productData => JsonConvert.DeserializeObject<Product>(productData))
                    .ToList();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération des produits du magasin.", error = ex.Message });
            }
        }

        // Endpoint to get all stores
        [HttpGet("all")]
        public IActionResult GetAllStores()
        {
            try
            {
                var db = _redisService.Database;
                var server = _redisService.GetServer();

                var storeKeys = server.Keys(pattern: "store:*").ToList();

                var stores = new List<Store>();

                foreach (var key in storeKeys)
                {
                    var type = db.KeyType(key);

                    if (type == RedisType.String)
                    {
                        var storeData = db.StringGet(key);
                        var store = JsonConvert.DeserializeObject<Store>(storeData);
                        stores.Add(store);
                    }
                }

                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération de tous les magasins.", error = ex.Message });
            }
        }

        // Endpoint to get a store by id
        [HttpGet("{storeId}")]
        public IActionResult GetStoreById(int storeId)
        {
            try
            {
                var db = _redisService.Database;
                var storeData = db.StringGet($"store:{storeId}");

                if (storeData.IsNullOrEmpty)
                {
                    return NotFound(new { message = "Magasin non trouvé." });
                }

                var store = JsonConvert.DeserializeObject<Store>(storeData);

                return Ok(store);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération du magasin.", error = ex.Message });
            }
        }
    }
}
