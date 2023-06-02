using System;
using System.Collections.Generic;
using System.Text;

namespace EU.Core.Enums
{
    public enum UserAgent
    {
        IOS = 0,
        Android = 1,
        Windows = 2,
        Linux
    }

    /// <summary>
    /// 资料修改模式
    /// </summary>
    public enum ModifyType
    {
        /// <summary>
        /// 新增模式。
        /// </summary>
        Add,
        /// <summary>
        /// 修改模式。
        /// </summary>
        Edit,
        /// <summary>
        /// 删除模式
        /// </summary>
        Delete,
        /// <summary>
        /// 插入模式
        /// </summary>
        Insert
    }
}
