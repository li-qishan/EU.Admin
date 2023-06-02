/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmConfig.cs
*
*功 能： N / A
* 类 名： SmConfig
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/4/17 20:55:08 SimonHsiao 初版
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
    /// 系统参数配置
    /// </summary>
    [Entity(TableCnName = "系统参数配置", TableName = "SmConfig")]
    public class SmConfig : Base.PersistPoco
    {

        /// <summary>
        /// 分组ID
        /// </summary>
        [Display(Name = "ConfigGroupId"), Description("分组ID")]
        public Guid? ConfigGroupId { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        [Display(Name = "ConfigName"), Description("参数名称")]
        public string ConfigName { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        [Display(Name = "ConfigCode"), Description("参数代码")]
        public string ConfigCode { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Display(Name = "ConfigValue"), Description("参数值")]
        public string ConfigValue { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [Display(Name = "InputType"), Description("参数类型")]
        public string InputType { get; set; }

        /// <summary>
        /// 配置内容
        /// </summary>
        [Display(Name = "AvailableValue"), Description("配置内容")]
        public string AvailableValue { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "Sequence"), Description("排序")]
        public int? Sequence { get; set; }
    }
}