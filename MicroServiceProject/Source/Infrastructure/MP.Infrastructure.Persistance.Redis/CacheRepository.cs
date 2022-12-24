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
        private readonly IDatabase _database;
        private readonly IConfiguration _configuration;

        public CacheRepository(int databaseNumber, IConfiguration configuration)
        {
            _configuration = configuration;
            _database = ConnectionMultiplexer.Connect(_configuration.GetSection("ConnectionStrings:RedisConnectionString").Value).GetDatabase(databaseNumber);
        }

        public T GetData<T>(string key)
        {
            var value = _database.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public void SetData<T>(string key, T value)
        {
            _database.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(24));
        }
        public void RemoveData(string key)
        {
            if (_database.KeyExists(key)) _database.KeyDelete(key);
        }
    }
}