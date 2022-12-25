using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace MP.Infrastructure.Persistance.Redis
{
    /// <summary>
    /// Cache repository
    /// </summary>
    public class CacheRepository : ICacheRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CacheRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_configuration.GetSection("ConnectionStrings:RedisConnectionString").Value);
        }

        public T GetData<T>(int dbNumber, string key)
        {
            IDatabase _database = _connectionMultiplexer.GetDatabase(dbNumber);
            var value = _database.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public void SetData<T>(int dbNumber, string key, T value)
        {
            IDatabase _database = _connectionMultiplexer.GetDatabase(dbNumber);
            _database.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(24));
        }
        public void RemoveData(int dbNumber, string key)
        {
            IDatabase _database = _connectionMultiplexer.GetDatabase(dbNumber);
            if (_database.KeyExists(key)) _database.KeyDelete(key);
        }
    }
}