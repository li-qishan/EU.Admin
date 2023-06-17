/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdOutOrderDetail.cs
*
*功 能： N / A
* 类 名： SdOutOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/3/10 14:14:41 SimonHsiao 初版
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
    [Entity(TableCnName = "出库单明细", TableName = "SdOutOrderDetail")]
    public class OutOrderDetail : Base.PersistPoco
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
        /// 出货来源
        /// </summary>
        [Display(Name = "OrderSource")]
        public string OrderSource { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        [Display(Name = "SalesOrderNo")]
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// 销售订单ID
        /// </summary>
        [Display(Name = "SalesOrderId")]
        public Guid? SalesOrderId { get; set; }

        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        [Display(Name = "SalesOrderDetailId")]
        public Guid? SalesOrderDetailId { get; set; }

        /// <summary>
        /// 计划发货
        /// </summary>
        [Display(Name = "ShipQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal ShipQTY { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        [Display(Name = "OutQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal OutQTY { get; set; }

        /// <summary>
        /// 退货数量
        /// </summary>
        [Display(Name = "ReturnQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal ReturnQTY { get; set; }

        /// <summary>
        /// 批次号ID
        /// </summary>
        [Display(Name = "BatchId")]
        public Guid? BatchId { get; set; }

        /// <summary>
        /// 客户物料编码
        /// </summary>
        [Display(Name = "CustomerMaterialCode")]
        public string CustomerMaterialCode { get; set; }

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
        /// 发货日期
        /// </summary>
        [Display(Name = "ShipDate")]
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [Display(Name = "DeliveryrDate")]
        public DateTime? DeliveryrDate { get; set; }

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

        /// <summary>
        /// 发货订单ID
        /// </summary>
        [Display(Name = "ShipOrderId")]
        public Guid? ShipOrderId { get; set; }

        /// <summary>
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        [Display(Name = "MaterialNo")]
        public string MaterialNo { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 货品规格
        /// </summary>
        [Display(Name = "MaterialSpecifications")]
        public string MaterialSpecifications { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }

        /// <summary>
        /// 发货订单明细ID
        /// </summary>
        [Display(Name = "ShipOrderDetailId")]
        public Guid? ShipOrderDetailId { get; set; }
    }
}