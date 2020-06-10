using CSRedis;
using DG.Blog.Domain.Configurations;
using DG.Blog.Domain.Shared;
using DG.Blog.ToolKits.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG.Blog.Redis
{
    /// <summary>
    ///     简易redis助手
    /// </summary>
    public class DGBlogRedisContext
    {
        private static readonly JsonSerializerSettings JsonOptions;

        static DGBlogRedisContext()
        {
            JsonOptions = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
            CSRedisClient.Serialize = Serialize;
            CSRedisClient.Deserialize = Deserialize;
        }

        /// <summary>
        ///     获取DataBase
        /// </summary>
        public CSRedisClient GetInstance()
        {
            return DefaultRedis.Instance;
        }

        #region ZScore

        /// <summary>
        /// 获取有序列表数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<long> ZCountAsync(string key = DGBlogConsts.Proxy.REDIS_KEY)
        {
            return Execute(() =>
            {
                var instance = GetInstance();
                return instance.ZCardAsync(key);
            });
        }

        /// <summary>
        /// 有序列表添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="score">初始分数</param>
        /// <returns></returns>
        public Task<long> ZAddAsync(object value, string key = DGBlogConsts.Proxy.REDIS_KEY, int score = DGBlogConsts.Proxy.INITIAL_SCORE)
        {
            return Execute(() =>
            {
                var instance = GetInstance();
                return instance.ZAddAsync(key, (score, value));
            });
        }

        /// <summary>
        ///     判断在缓存中是否存在该key的缓存数据
        /// </summary>
        public Task<bool> ZExistsAsync(object value, string key = DGBlogConsts.Proxy.REDIS_KEY)
        {
            return Execute(async () => (await GetInstance().ZScoreAsync(key, value)) != null);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Task<string[]> ZGetAll(string key = DGBlogConsts.Proxy.REDIS_KEY, int min = DGBlogConsts.Proxy.MIN_SCORE, int max = DGBlogConsts.Proxy.MAX_SCORE)
        {
            return Execute(() =>
            {
                var instance = GetInstance();
                return instance.ZRevRangeByScoreAsync(key, min, max);
            });
        }

        public Task<string[]> ZBatchGet(long start, long stop, string key = DGBlogConsts.Proxy.REDIS_KEY)
        {
            return Execute(() =>
            {
                var instance = GetInstance();
                return instance.ZRevRangeAsync(key, start, stop);
            });
        }

        public Task<long> ZMaxAsync(object value)
        {
            return Execute(() =>
            {
                var instance = GetInstance();
                return instance.ZAddAsync(DGBlogConsts.Proxy.REDIS_KEY, (DGBlogConsts.Proxy.MAX_SCORE, value));
            });
        }

        public Task<bool> ZDecreaseAsync(string value)
        {
            return Execute(async () =>
            {
                var instance = GetInstance();
                var score = await instance.ZScoreAsync(DGBlogConsts.Proxy.REDIS_KEY, value);
                if (score.HasValue && score > DGBlogConsts.Proxy.MIN_SCORE)
                {
                    await instance.ZIncrByAsync(DGBlogConsts.Proxy.REDIS_KEY, value, -1);
                    return true;
                }
                else
                {
                    await instance.ZRemAsync(DGBlogConsts.Proxy.REDIS_KEY, value);
                    return false;
                }
            });
        }

        /// <summary>
        /// 随机获取有效代理，首先尝试获取最高分数代理，如果不存在，按照排名获取，否则异常
        /// </summary>
        /// <returns></returns>
        public Task<string> ZRandomAsync(string key = DGBlogConsts.Proxy.REDIS_KEY)
        {
            return Execute(async () =>
            {
                var instance = GetInstance();
                var result = await instance.ZRangeByScoreAsync(key, DGBlogConsts.Proxy.MAX_SCORE, DGBlogConsts.Proxy.MAX_SCORE);
                if (result != null && result.Any())
                {
                    return result[new Random().Next(result.Length)];
                }
                else
                {
                    result = await instance.ZRevRangeAsync(key, DGBlogConsts.Proxy.MIN_SCORE, DGBlogConsts.Proxy.MAX_SCORE);
                    if (result != null && result.Any())
                    {
                        return result[new Random().Next(result.Length)];
                    }
                    else
                        return null;
                    //throw new Exception("No ProxyIP from redis!");
                }
            });
        }

        #endregion ZScore

        /// <summary>
        ///     异步设置
        /// </summary>
        public Task SetAsync(string key, object value, int expireSec = -1)
        {
            return Execute(() =>
            {
                var instance = GetInstance();
                return instance.SetAsync(key, value, expireSec);
            });
        }

        /// <summary>
        ///     判断在缓存中是否存在该key的缓存数据
        /// </summary>
        public Task<bool> ExistsAsync(string key)
        {
            return Execute(() => GetInstance().ExistsAsync(key));
        }

        /// <summary>
        ///     移除指定key的缓存
        /// </summary>
        public Task<bool> RemoveAsync(string key)
        {
            return Execute(async () =>
            {
                var count = await GetInstance().DelAsync(key);
                return count > 0;
            });
        }

        /// <summary>
        ///     异步获取
        /// </summary>
        public Task<T> GetAsync<T>(string key)
        {
            return Execute(async () =>
            {
                var instance = GetInstance();
                instance.CurrentDeserialize = (p, t) => Deserialize(p);
                var value = await instance.GetAsync<T>(key);
                return value;
            });
        }

        /// <summary>
        ///     模糊获取批量数据
        /// </summary>
        public async Task<IEnumerable<T>> BatchGetAsync<T>(string keyPattern)
        {
            var keys = GetInstance().Keys(keyPattern);
            var list = new List<T>();
            foreach (var key in keys)
            {
                var model = await GetAsync<T>(key);
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        ///     异步设置redis Key的时间
        /// </summary>
        public Task<bool> ExpireAsync(string key, int expireSec)
        {
            return GetInstance().ExpireAsync(key, expireSec);
        }

        /// <summary>
        ///     异步实现递增
        /// </summary>
        /// <returns></returns>
        public Task<long> IncrementAsync(string key, int expireSec = 30)
        {
            return Execute(async () =>
            {
                var db = GetInstance();
                var res = await db.IncrByAsync(key);
                if (expireSec > 0)
                {
                    var expire = TimeSpan.FromSeconds(expireSec);
                    await db.ExpireAsync(key, expire);
                }

                return res;
            });
        }

        /// <summary>
        ///     发布消息
        /// </summary>
        public Task PublishAsync<T>(string channel, T message)
        {
            return Execute(async () => { await GetInstance().PublishAsync(channel, Serialize(message)); });
        }

        /// <summary>
        ///     订阅并消费消息
        /// </summary>
        public void Subscribe<T>(string channelFrom, Action<T> action)
        {
            Execute(() =>
                GetInstance().Subscribe((channelFrom, message =>
                {
                    var res = Deserialize(message.Body, typeof(T));
                    action((T)res);
                }
            ))
            );
        }

        private static string Serialize(object o)
        {
            if (o == null) return null;
            if (o is string || o is ValueType)
                return Convert.ToString(o);
            return JsonConvert.SerializeObject(o, JsonOptions);
        }

        private static object Deserialize(string data, Type type)
        {
            if (string.IsNullOrEmpty(data)) return default;
            if (type == typeof(string) || typeof(ValueType).IsAssignableFrom(type))
            {
                object value;
                if (type.IsEnum)
                {
                    value = Enum.Parse(type, data, true);
                }
                else if (type.IsGenericType)
                {
                    var subType = type.GenericTypeArguments[0];
                    if (subType.IsEnum)
                        value = Enum.Parse(subType, data, true);
                    else
                        value = Convert.ChangeType(data, subType);
                }
                else
                {
                    value = Convert.ChangeType(data, type);
                }

                return value;
            }

            return JsonConvert.DeserializeObject(data, type, JsonOptions);
        }

        private static object Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data)) return default;
            return JsonConvert.DeserializeObject(data, JsonOptions);
        }

        private T Execute<T>(Func<T> func)
        {
            if (func == null) return default;

            try
            {
                return func.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, "Redis访问出现错误");
                return default;
            }
        }

        private async Task<T> Execute<T>(Func<Task<T>> func)
        {
            if (func == null) return default;
            try
            {
                return await func.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, "Redis访问出现错误");
                return default;
            }
        }
    }

    public class DefaultRedis : RedisHelper<DefaultRedis>
    {
    }
}