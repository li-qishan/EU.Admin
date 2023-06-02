using EU.Core.CacheManager;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.Model.System.Privilege;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EU.Core.Module
{
    public class FunctionPrivilege
    {
        #region 获取模块
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SmFunctionPrivilege> GetList(string ModuleId)
        {
            List<SmFunctionPrivilege> moduleList = new RedisCacheService(2).Get<List<SmFunctionPrivilege>>(CacheKeys.SmFunctionPrivilege.ToString(), ModuleId);
            if (moduleList == null)
            {
                string sql = "SELECT A.* FROM SmFunctionPrivilege A WHERE A.SmModuleId='{0}' AND IsDeleted='false'";
                sql = string.Format(sql, ModuleId);
                moduleList = DBHelper.Instance.QueryList<SmFunctionPrivilege>(sql);
                new RedisCacheService(2).AddObject(CacheKeys.SmFunctionPrivilege.ToString(), ModuleId, moduleList);
            }
            return moduleList;
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            new RedisCacheService(2).Remove(CacheKeys.SmFunctionPrivilege.ToString());
        }
    }
}
