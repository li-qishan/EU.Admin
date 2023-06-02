/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdShipOrderDetail.cs
*
*功 能： N / A
* 类 名： SdShipOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/3/5 11:12:28 SimonHsiao 初版
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
    [Entity(TableCnName = "发货单明细", TableName = "SdShipOrderDetail")]
    public class ShipOrderDetail : Base.PersistPoco
    {


        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 销售订单ID
        /// </summary>
        [Display(Name = "SalesOrderId")]
        public Guid? SalesOrderId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        [Display(Name = "SalesOrderDetailId")]
        public Guid? SalesOrderDetailId { get; set; }

        /// <summary>
        /// 货品编号
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }

        /// <summary>
        /// 未发数量
        /// </summary>
        [Display(Name = "NoShipQTY")]
        public decimal? NoShipQTY { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        [Display(Name = "ShipQTY")]
        public decimal? ShipQTY { get; set; }

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
    }
}