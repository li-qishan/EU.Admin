﻿/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
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
    public class ShipOrderExtend
    {
        public Guid? ID { get; set; }
        public Guid? OrderId { get; set; }

        public string OrderNo { get; set; }

        public DateTime? CreatedTime { get; set; }

        public int SerialNumber { get; set; }

        public Guid? MaterialId { get; set; }
        public string MaterialNo { get; set; }

        public string MaterialName { get; set; }

        public string MaterialSpecifications { get; set; }
        public Guid? UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal QTY { get; set; }
        public decimal WaitQTY { get; set; }
        public decimal ShipQTY { get; set; }

        public decimal OutQTY { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 未税金额
        /// </summary>
        public decimal NoTaxAmount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        public decimal TaxAmount { get; set; }
        /// <summary>
        /// 含税金额
        /// </summary>
        public decimal TaxIncludedAmount { get; set; }

        public string CustomerMaterialCode { get; set; }
        public DateTime? DeliveryrDate { get; set; }

        public Guid? ShipOrderId { get; set; }
        public Guid? ShipOrderDetailId { get; set; }
        public Guid? StockId { get; set; }
        public Guid? GoodsLocationId { get; set; }

        public string StockName { get; set; }
        public string GoodsLocationName { get; set; }
        public int DecimalPlaces { get; set; }
        public decimal Step { get; set; }
        public decimal Min { get; set; }

        public string OrderSource { get; set; }
        public Guid? SalesOrderId { get; set; }
        public Guid? SalesOrderDetailId { get; set; }
        public decimal InventoryQTY { get; set; }

        public List<Stock> StockList { get; set; }
        public List<GoodsLocation> GoodsLocationList { get; set; }
    }
}