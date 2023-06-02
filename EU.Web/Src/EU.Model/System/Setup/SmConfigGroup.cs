/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmConfigGroup.cs
*
*功 能： N / A
* 类 名： SmConfigGroup
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/4/17 20:43:09 SimonHsiao 初版
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
    /// 系统参数分组
    /// </summary>
    [Entity(TableCnName = "系统参数分组", TableName = "SmConfigGroup")]
    public class SmConfigGroup : Base.PersistPoco
    {

        /// <summary>
        /// 上级ID
        /// </summary>
        [Display(Name = "ParentId"), Description("上级ID")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name"), Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "Type"), Description("类型")]
        public string Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "Sequence"), Description("排序")]
        public int? Sequence { get; set; }
    }
}