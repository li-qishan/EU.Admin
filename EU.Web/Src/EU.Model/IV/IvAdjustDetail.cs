/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* IvAdjustDetail.cs
*
*功 能： N / A
* 类 名： IvAdjustDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/15 22:02:13 SimonHsiao 初版
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
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model
{
    /// <summary>
    /// 库存调整单明细
    /// </summary>
    [Entity(TableCnName = "库存调整单明细", TableName = "IvAdjustDetail")]
    public class IvAdjustDetail : Base.PersistPoco
    {


        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 调整类型，调增/调减（调增就是增加库存，调减就是减少库存）
        /// </summary>
        [Display(Name = "AdjustType")]
        public string AdjustType { get; set; }

        /// <summary>
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "QTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? QTY { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "Price"), Column(TypeName = "decimal(20,2)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "Amount"), Column(TypeName = "decimal(20,2)")]
        public decimal? Amount { get; set; }

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
        /// 批次号
        /// </summary>
        [Display(Name = "BatchNo")]
        public string BatchNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark1")]
        public string ExtRemark1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark2")]
        public string ExtRemark2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark3")]
        public string ExtRemark3 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark4")]
        public string ExtRemark4 { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IvAdjustDetailExtend : IvAdjustDetail
    {
        /// <summary>
        /// 货品名称
        /// </summary>
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GoodsLocationName { get; set; }
    }
}