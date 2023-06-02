/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsBOM.cs
*
*功 能： N / A
* 类 名： PsBOM
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/29 22:25:20 SimonHsiao 初版
*
* Copyright(c) 2021 SUZHOU EU Corporation. All Rights Reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using EU.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    [Entity(TableCnName = "BOM", TableName = "PsBOM")]
    public class BOM : Base.PersistPoco
    {
        /// <summary>
        /// 货品编号
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 制造车间ID
        /// </summary>
        [Display(Name = "WorkShopId")]
        public Guid? WorkShopId { get; set; }

        /// <summary>
        /// BOM版本
        /// </summary>
        [Display(Name = "Version")]
        public string Version { get; set; }

        /// <summary>
        /// 批量
        /// </summary>
        [Display(Name = "BulkQty")]
        public int BulkQty { get; set; }

        /// <summary>
        /// 是否最新
        /// </summary>
        [Display(Name = "IsLatest")]
        public bool IsLatest { get; set; } = true;

        /// <summary>
        /// 工序流程
        /// </summary>
        [Display(Name = "Process")]
        public string Process { get; set; }
    }
}