using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ecommerce.Models;
using Ecommerce.Services;
using Ecommerce.Properties.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

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

        [HttpPost("add")]
        public IActionResult AddProductToStore([FromBody] StoreProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var serializedProduct = JsonConvert.SerializeObject(request.Product);
                _redisService.Database.ListRightPush($"store:{request.StoreId}:products", serializedProduct);

                return Ok(new { message = "Produit ajouté avec succès dans Redis pour le magasin correspondant." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur dans l'ajout du produit dans Redis.", error = ex.Message });
            }
        }

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

        // New method to retrieve all stores
        [HttpGet("all")]
        public IActionResult GetAllStores()
        {
            try
            {
                var db = _redisService.Database;

                // Get the Redis server instance
                var server = _redisService.GetServer();

                // Get all keys that match the "store:*" pattern
                var storeKeys = server.Keys(pattern: "store:*").ToList();

                var stores = new List<Store>();

                foreach (var key in storeKeys)
                {
                    // Check the data type of the key
                    var type = db.KeyType(key);

                    // Only handle the keys that store string data
                    if (type == RedisType.String)
                    {
                        var storeData = db.StringGet(key); // Retrieve store data as JSON from Redis
                        var store = JsonConvert.DeserializeObject<Store>(storeData); // Deserialize into Store object
                        stores.Add(store);
                    }
                    else
                    {
                        // Log or handle the case where the key contains a different type of data
                        Console.WriteLine($"Key {key} is not a string, skipping...");
                    }
                }

                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la récupération de tous les magasins.", error = ex.Message });
            }
        }



    }
}
