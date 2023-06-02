/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmApi.cs
*
*功 能： N / A
* 类 名： SmApi
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/5/8 1:42:45 SimonHsiao 初版
*
* Copyright(c) 2022 SUZHOU EU Corporation. All Rights Reserved.
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
    /// API接口授权
    /// </summary>
    [Entity(TableCnName = "API接口授权", TableName = "SmApi")]
    public class SmApi : Base.PersistPoco
    {

        /// <summary>
        /// API名称
        /// </summary>
        [Display(Name = "ApiName"), Description("API名称")]
        public string ApiName { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        [Display(Name = "AppId"), Description("应用ID")]
        public string AppId { get; set; }

        /// <summary>
        /// APP密钥
        /// </summary>
        [Display(Name = "AppSecret"), Description("APP密钥")]
        public string AppSecret { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        [Display(Name = "BeginDate"), Description("生效日期")]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        [Display(Name = "EndDate"), Description("失效日期")]
        public DateTime? EndDate { get; set; }
    }
}