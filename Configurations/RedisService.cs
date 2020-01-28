using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.Configurations
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        private readonly string _redisHost;
        private readonly int _redisPort;

        public RedisService(IConfiguration config)
        {
            _redisHost = config["Redis:Host"];
            _redisPort = Convert.ToInt32(config["Redis:Port"]);
            this.Connect();
        }
        public bool Set(string key, string value)
        {
            this.Connect();
            var db = _redis.GetDatabase();
            return  db.StringSet(key, value);
        }

        public string Get(string key)
        {
            var db = _redis.GetDatabase();
            return db.StringGet(key);
        }
        public void Connect()
        {
            try
            {

                var configString = $"{_redisHost}:{_redisPort},password=5RcDHdeajI6yXTcfkNmZwfVyegGn4x5I,connectRetry=5,abortConnect=false";
                _redis = ConnectionMultiplexer.Connect(configString);
            }
            catch (RedisConnectionException err)
            {
                Console.WriteLine(err.ToString());
                throw err;
            }
            Console.WriteLine("Connected to Redis");
        }

        public void CreateUserCache(string userId)
        {
            var db = _redis.GetDatabase();
            db.HashSet(userId, new HashEntry[100]);
        }

        public void SetPermission(string keyName, HashEntry[] permissions)
        {
            var db = _redis.GetDatabase();
            db.KeyDelete(keyName);
            db.KeyExpire(keyName, TimeSpan.FromMinutes(60));
            db.HashSet(keyName, permissions);
           
        }

        public string GetUserKey(string userId,string keyName)
        {
            var db = _redis.GetDatabase();
            return db.HashGet(userId, keyName).ToString();
        }

    }
}
