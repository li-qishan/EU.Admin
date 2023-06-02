/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdOrder.cs
*
*功 能： N / A
* 类 名： SdOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/26 15:44:13 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    public class ReturnOrderExtend
    {
        public Guid? ID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 出货来源
        /// </summary>
        [Display(Name = "OrderSource")]
        public string OrderSource { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        [Display(Name = "OutOrderNo")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 出库订单ID
        /// </summary>
        [Display(Name = "OutOrderId")]
        public Guid? OutOrderId { get; set; }

        /// <summary>
        /// 出库订单明细ID
        /// </summary>
        [Display(Name = "OutOrderDetailId")]
        public Guid? OutOrderDetailId { get; set; }

        /// <summary>
        /// 销售单号
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
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialNo { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string MaterialSpecifications { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public decimal OutQTY { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// 退货数量
        /// </summary>
        public decimal ReturnQTY { get; set; }

        public string CustomerMaterialCode { get; set; }

        public string UnitName { get; set; }

        /// <summary>
        /// 当前库存
        /// </summary>
        public decimal InventoryQTY { get; set; }
        public Guid? StockId { get; set; }
        public string StockName { get; set; }

        public Guid? GoodsLocationId { get; set; }
        public string GoodsLocationName { get; set; }

        public DateTime? OutTime { get; set; }

        public int DecimalPlaces { get; set; }
        public decimal Step { get; set; }
        public decimal Min { get; set; }

        /// <summary>
        /// 是否实物退货
        /// </summary>
        [Display(Name = "IsEntity")]
        public bool IsEntity { get; set; }

        public List<Stock> StockList { get; set; }
        public List<GoodsLocation> GoodsLocationList { get; set; }
    }

}