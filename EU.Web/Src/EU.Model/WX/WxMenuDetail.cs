/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* WxMenuDetail.cs
*
*功 能： N / A
* 类 名： WxMenuDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/6/22 16:40:52 SimonHsiao 初版
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
    /// 微信菜单明细
    /// </summary>
    [Entity(TableCnName = "微信菜单明细", TableName = "WxMenuDetail")]
    public class WxMenuDetail : Base.PersistPoco
    {

        /// <summary>
        /// 微信配置ID
        /// </summary>
        [Display(Name = "ConfigId"), Description("微信配置ID")]
        public Guid? ConfigId { get; set; }

        /// <summary>
        /// 微信菜单ID
        /// </summary>
        [Display(Name = "MenuId"), Description("微信菜单ID")]
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [Display(Name = "MenuType"), Description("菜单类型")]
        public string MenuType { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        [Display(Name = "MenuName"), Description("菜单名")]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单值
        /// </summary>
        [Display(Name = "MenuTypeValue"), Description("菜单值")]
        public string MenuTypeValue { get; set; }
    }
}