using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
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

        public T GetData<T>(string key)
        {
            IDatabase database = _connectionMultiplexer.GetDatabase(Convert.ToInt32(_configuration.GetSection($"RedisTableNumbers:{typeof(T).GetType().Name}").Value));
            RedisValue value = database.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public void SetData<T>(string key, T value)
        {
            IDatabase database = _connectionMultiplexer.GetDatabase(Convert.ToInt32(_configuration.GetSection($"RedisTableNumbers:{typeof(T).GetType().Name}").Value));
            database.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(24));
        }

        public void RemoveData(BaseEntity baseEntity, string key)
        {
            IDatabase database = _connectionMultiplexer.GetDatabase(Convert.ToInt32(_configuration.GetSection($"RedisTableNumbers:{baseEntity.GetType().Name}").Value));
            if (database.KeyExists(key)) database.KeyDelete(key);
        }
    }
}