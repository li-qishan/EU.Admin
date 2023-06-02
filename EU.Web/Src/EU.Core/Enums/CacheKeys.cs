using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Core.Enums
{
    public enum CacheKeys
    {
        /// <summary>
        /// 模块信息
        /// </summary>
        SmModule,
        /// <summary>
        /// 模块SQL信息
        /// </summary>
        SmModuleSql,
        SmModuleColumn,
        /// <summary>
        /// 功能权限定义
        /// </summary>
        SmFunctionPrivilege,
        Sm_Module_Sql_All_Column,
        Sm_Toolbar_Priv,
        Sm_Module_Toolbar_Priv,
        Sm_User_Module_Temp_Priv,
        Sm_Interface_Config,
        SmLov,
        ProvinceCity,
        /// <summary>
        /// 系统参数
        /// </summary>
        SmConfig
    }
}
