using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using EU.Core.Enums;
using EU.Core.CacheManager;
using EU.Model.System;
using System.Threading.Tasks;
using Autofac.Core;

namespace EU.Core.Utilities
{
    public class LOVHelper
    {
        public static RedisCacheService redis = new RedisCacheService(3);

        #region 获取值列表
        /// <summary>
        /// 获取值列表
        /// </summary>
        /// <param name="moduleCode">值代码</param>
        /// <returns></returns>
        public static List<LovInfo> GetLovList(string code)
        {
            List<LovInfo> cache = redis.Get<List<LovInfo>>(CacheKeys.SmLov.ToString(), code);
            if (cache == null)
            {
                Init();
                cache = redis.Get<List<LovInfo>>(CacheKeys.SmLov.ToString(), code);
            }
            return cache ?? new List<LovInfo>();
        }
        public static async Task<List<LovInfo>> GetLovListAsync(string code)
        {
            List<LovInfo> cache = await redis.GetAsync<List<LovInfo>>(CacheKeys.SmLov.ToString(), code);
            if (cache == null)
            {
                await InitAsync();
                cache = await redis.GetAsync<List<LovInfo>>(CacheKeys.SmLov.ToString(), code);
            }
            return cache ?? new List<LovInfo>();
        }
        #endregion

        /// <summary>
        /// 初始化系统参数
        /// </summary>
        public static void Init()
        {

            redis.Remove(CacheKeys.SmLov.ToString());

            string sql = "SELECT LovCode FROM SmLov where IsDeleted='false'";
            List<SmLov> lov = DBHelper.Instance.QueryList<SmLov>(sql, null);
            List<LovInfo> cache = new List<LovInfo>();
            sql = "SELECT [Value], [Text], LovCode FROM SmLovV ORDER BY TaxisNo ASC";
            cache = DBHelper.Instance.QueryList<LovInfo>(sql, null);

            foreach (SmLov item in lov)
            {
                List<LovInfo> list = cache.Where(x => x.LovCode == item.LovCode).ToList();
                redis.AddObject(CacheKeys.SmLov.ToString(), item.LovCode, list);
            }
        }

        public static async Task InitAsync()
        {

            redis.Remove(CacheKeys.SmLov.ToString());

            string sql = "SELECT LovCode FROM SmLov where IsDeleted='false'";
            List<SmLov> lov = await DBHelper.Instance.QueryListAsync<SmLov>(sql, null);
            List<LovInfo> cache = new List<LovInfo>();
            sql = "SELECT [Value], [Text], LovCode FROM SmLovV ORDER BY TaxisNo ASC";
            cache = await DBHelper.Instance.QueryListAsync<LovInfo>(sql, null);

            foreach (SmLov item in lov)
            {
                List<LovInfo> list = cache.Where(x => x.LovCode == item.LovCode).ToList();
                await redis.AddObjectAsync(CacheKeys.SmLov.ToString(), item.LovCode, list);
            }
        }
    }

    public class LovInfo
    {
        public string Value { get; set; }

        public string Text { get; set; }
        public string LovCode { get; set; }

    }
}

