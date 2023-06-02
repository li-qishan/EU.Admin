/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* MfInOrderDetail.cs
*
*功 能： N / A
* 类 名： MfInOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/6/9 21:43:41 SimonHsiao 初版
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
    [Entity(TableCnName = "工模治具入账明细", TableName = "MfInOrderDetail")]
    public class MfInOrderDetail : Base.PersistPoco
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
        /// 模具ID
        /// </summary>
        [Display(Name = "MouldId")]
        public Guid? MouldId { get; set; }

        /// <summary>
        /// 币别ID
        /// </summary>
        [Display(Name = "CurrencyId")]
        public Guid? CurrencyId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal QTY { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Display(Name = "Price")]
        public decimal Price { get; set; }

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
        [Display(Name = "BatchId")]
        public Guid? BatchId { get; set; }
    }
}