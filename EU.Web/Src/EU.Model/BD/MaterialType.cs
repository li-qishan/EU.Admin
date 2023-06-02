/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdMaterialType.cs
*
*功 能： N / A
* 类 名： BdMaterialType
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/11/29 21:18:43 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    public class MaterialType : Base.PersistPoco
    {


        /// <summary>
        /// 类型编号
        /// </summary>
        [Display(Name = "类型编号")]
        public string MaterialTypeNo { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Display(Name = "类型名称")]
        public string MaterialTypeNames { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int TaxisNo { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Display(Name = "上级ID")]
        public Guid? ParentTypeId { get; set; }
    }
}