/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PoInOrderDetail.cs
*
*功 能： N / A
* 类 名： PoInOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/13 17:26:47 SimonHsiao 初版
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
    /// 采购入库单明细
    /// </summary>
    [Entity(TableCnName = "采购入库单明细", TableName = "PoInOrderDetail")]
    public class InOrderDetail : Base.PersistPoco
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
        /// 购单、到货通知单
        /// </summary>
        [Display(Name = "OrderSource")]
        public string OrderSource { get; set; }

        /// <summary>
        /// 来源订单ID
        /// </summary>
        [Display(Name = "SourceOrderId")]
        public Guid? SourceOrderId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        [Display(Name = "SourceOrderNo")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 来源订单明细ID
        /// </summary>
        [Display(Name = "SourceOrderDetailId")]
        public Guid? SourceOrderDetailId { get; set; }

        /// <summary>
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

   

        /// <summary>
        /// 采购数量
        /// </summary>
        [Display(Name = "PurchaseQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal PurchaseQTY { get; set; }

        /// <summary>
        /// 到货数量
        /// </summary>
        [Display(Name = "ArrivalQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal ArrivalQTY { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        [Display(Name = "InQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal InQTY { get; set; }

        /// <summary>
        /// 退货数量
        /// </summary>
        [Display(Name = "ReturnQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal ReturnQTY { get; set; }

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
        /// 批次号ID
        /// </summary>
        [Display(Name = "BatchNo")]
        public string BatchNo { get; set; }

        /// <summary>
        /// 预定交期
        /// </summary>
        [Display(Name = "ReserveDeliveryTime")]
        public DateTime? ReserveDeliveryTime { get; set; }

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
    /// 采购入库单明细
    /// </summary>
    public class InOrderDetailExtend : InOrderDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public int ROWNUM { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public string MaterialNo { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal MaxInQTY { get; set; }
    }
}