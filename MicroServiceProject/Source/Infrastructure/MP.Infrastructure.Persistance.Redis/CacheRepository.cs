using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

        public List<T> GetAll<T>()
        {
            List<T> data = new List<T>();
            ConfigurationOptions options = ConfigurationOptions.Parse(_configuration.GetSection("ConnectionStrings:RedisConnectionString").Value);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
            IDatabase database = _connectionMultiplexer.GetDatabase(Convert.ToInt32(_configuration.GetSection($"RedisTableNumbers:{typeof(T).GetType().Name}").Value));
            EndPoint endPoint = connection.GetEndPoints().First();
            RedisKey[] keys = connection.GetServer(endPoint).Keys(pattern: "*").ToArray();
            foreach (var key in keys)
            {
                data.Add(JsonConvert.DeserializeObject<T>(database.StringGet(key)));
            }
            return data;
        }
    }
}