using StackExchange.Redis;
using System.Linq;

namespace Ecommerce.Services
{
    public class RedisService : IDisposable
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDb;

        public RedisService()
        {
            // Connexion au serveur Redis
            _redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
            _redisDb = _redisConnection.GetDatabase();
        }

        public IDatabase Database => _redisDb;

        // Method to access the Redis server instance
        public IServer GetServer()
        {
            return _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
        }

        public void Dispose()
        {
            _redisConnection?.Dispose();
        }
    }
}
