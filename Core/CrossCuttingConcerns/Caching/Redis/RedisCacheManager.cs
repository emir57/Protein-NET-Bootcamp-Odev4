using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;

namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly RedisEndpoint _redisEndpoint;
        public RedisCacheManager(IConfiguration configuration)
        {
            RedisConfiguration redisConfiguration = configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>();
            _redisEndpoint = new RedisEndpoint(redisConfiguration.Host, redisConfiguration.Port, redisConfiguration.Password);
        }
        public void Add(string key, object value, int duration)
        {
            dynamic dynamicValue = value;
            RedisInvoker(x => x.Add(key, dynamicValue, TimeSpan.FromMinutes(duration)));
        }

        public object Get(string key)
        {
            object result = default;
            RedisInvoker(x => result = x.Get(key));
            return result;
        }

        public T Get<T>(string key)
        {
            T result = default(T);
            RedisInvoker(x => result = x.Get<T>(key));
            return result;
        }

        public bool IsAdd(string key)
        {
            bool isAdd = false;
            RedisInvoker(x => isAdd = x.ContainsKey(key));
            return isAdd;
        }

        public void Remove(string key)
        {
            RedisInvoker(x => x.Remove(key));
        }

        public void RemoveByPattern(string pattern)
        {
            RedisInvoker(x => x.RemoveByPattern(pattern));
        }
        private void RedisInvoker(Action<RedisClient> redisAction)
        {
            using (var client = new RedisClient(_redisEndpoint))
            {
                redisAction.Invoke(client);
            }
        }
    }
}
