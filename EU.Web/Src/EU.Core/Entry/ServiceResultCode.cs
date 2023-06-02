using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Core.Entry
{
    /// <summary>
    /// 服务层响应码枚举
    /// </summary>
    public enum ServiceResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 10000,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 10001,

        /// <summary>
        /// 登录失败
        /// </summary>
        LoginFailed = 10002,

        /// <summary>
        /// 数据库操作失败
        /// </summary>
        SqlException = 10003,

        /// <summary>
        /// 字段重复
        /// </summary>
        FieldRepeat = 10004,

        /// <summary>
        /// 模板不存在
        /// </summary>
        TmplError = 10005,

        /// <summary>
        /// 允许强制登陆
        /// </summary>
        AllowForce = 10006,

        /// <summary>
        /// Token未授权
        /// </summary>
        Unauthorized = 10007
    }
}
