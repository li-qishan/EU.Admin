/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* WxMenu.cs
*
*功 能： N / A
* 类 名： WxMenu
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/6/4 19:34:43 SimonHsiao 初版
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
    /// 微信菜单
    /// </summary>
    [Entity(TableCnName = "微信菜单", TableName = "WxMenu")]
    public class WxMenu : Base.PersistPoco
    {

        /// <summary>
        /// 微信配置ID
        /// </summary>
        [Display(Name = "ConfigId"), Description("微信配置ID")]
        public Guid? ConfigId { get; set; }

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

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "TaxisNo"), Description("排序号")]
        public int? TaxisNo { get; set; }

        /// <summary>
        /// 是否身份认证
        /// </summary>
        [Display(Name = "isAuth"), Description("是否身份认证")]
        public bool? isAuth { get; set; }
    }
}