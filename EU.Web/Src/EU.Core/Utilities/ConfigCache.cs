using EU.Core.CacheManager;
using EU.Core.Enums;
using EU.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Core.Utilities
{
    /// <summary>
    /// 系统参数方法类
    /// </summary>
    public class ConfigCache
    {
        public static RedisCacheService redis = new RedisCacheService(3);
        public static string GetValue(string key)
        {
            SmConfig value = redis.Get<SmConfig>(CacheKeys.SmConfig.ToString(), key);
            if (value == null)
            {
                Init();
                value = new RedisCacheService(3).Get<SmConfig>(CacheKeys.SmConfig.ToString(), key);
                return value?.ConfigValue;
            }
            else
                return value.ConfigValue;
        }

        //public static SmConfig GetSmConfig(string code)
        //{
        //    SmConfig cache = new RedisCacheService(2).Get<SmConfig>(CacheKeys.SmConfig.ToString(), code);
        //    if (cache == null)
        //    {
        //        Init();
        //        cache = new RedisCacheService(2).Get<SmConfig>(CacheKeys.SmConfig.ToString(), code);
        //    }
        //    return cache;
        //}

        /// <summary>
        /// 初始化系统参数
        /// </summary>
        public static void Init()
        {
            redis.Remove(CacheKeys.SmConfig.ToString());

            string sql = "SELECT * FROM SmConfig WHERE IsActive='true' AND IsDeleted='false'";
            List<SmConfig> list = DBHelper.Instance.QueryList<SmConfig>(sql);
            foreach (SmConfig item in list)
                redis.AddObject(CacheKeys.SmConfig.ToString(), item.ConfigCode, item);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">参数key</param>
        /// <param name="item">参数实体</param>
        public static void Add(string key, SmConfig item = null)
        {
            redis.AddObject(CacheKeys.SmConfig.ToString(), key, item);
        }
    }
}
