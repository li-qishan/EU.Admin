/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdMaterialIVChange.cs
*
*功 能： N / A
* 类 名： BdMaterialIVChange
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/4/12 14:42:10 SimonHsiao 初版
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
    [Entity(TableCnName = "", TableName = "BdMaterialIVChange")]
    public class MaterialIVChange : Base.PersistPoco
    {
        /// <summary>
        /// 材质编号
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Display(Name = "StockId")]
        public Guid? StockId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        [Display(Name = "GoodsLocationId")]
        public Guid? GoodsLocationId { get; set; }

        /// <summary>
        /// 变更数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal? QTY { get; set; }

        /// <summary>
        /// 变前数量
        /// </summary>
        [Display(Name = "BeforeQTY")]
        public decimal? BeforeQTY { get; set; }

        /// <summary>
        /// 变后数量
        /// </summary>
        [Display(Name = "AfterQTY")]
        public decimal? AfterQTY { get; set; }

        /// <summary>
        /// 变化类型
        /// </summary>
        [Display(Name = "ChangeType")]
        public string ChangeType { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 订单明细ID
        /// </summary>
        [Display(Name = "OrderDetailId")]
        public Guid? OrderDetailId { get; set; }
    }
}