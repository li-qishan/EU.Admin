/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* RmType.cs
*
*功 能： N / A
* 类 名： SmApiLog
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2023/6/24 0:05:25 SimonHsiao 初版
*
* Copyright(c) 2023/6/24 0:05:25Year SUZHOU EU Corporation. All Rights Reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using EU.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{

    /// <summary>
    /// SmApiLog
    /// </summary>
    [Entity(TableCnName = "SmApiLog", TableName = "SmApiLog")]
    public class SmApiLog : Base.PersistPoco
    {



        /// <summary>
        /// 请求地址
        /// </summary>
        [Display(Name = "Path"), Description("请求地址")]
        public string Path { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [Display(Name = "Method"), Description("请求方式")]
        public string Method { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP"), Description("IP")]
        public string IP { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Display(Name = "Source"), Description("来源")]
        public string Source { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        [Display(Name = "Content"), Description("请求内容")]
        public string Content { get; set; }
    }
}