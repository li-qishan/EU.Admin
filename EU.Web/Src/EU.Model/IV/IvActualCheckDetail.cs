/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* IvActualCheckDetail.cs
*
*功 能： N / A
* 类 名： IvActualCheckDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/4/22 17:12:33 SimonHsiao 初版
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
    [Entity(TableCnName = "实际盘点单明细", TableName = "IvActualCheckDetail")]
    public class IvActualCheckDetail : Base.PersistPoco
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
        /// 盘点名称
        /// </summary>
        [Display(Name = "CheckName")]
        public string CheckName { get; set; }

        /// <summary>
        /// 货品ID
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
        /// 库存数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal QTY { get; set; }

        /// <summary>
        /// 实盘数量
        /// </summary>
        [Display(Name = "ActualQTY")]
        public decimal ActualQTY { get; set; }

        /// <summary>
        /// 盘点盈亏，实际盘点数量-账面库存数量，结果为正数，则表示盘盈，结果为负数，则表示盘亏
        /// </summary>
        [Display(Name = "DiffQTY")]
        public decimal DiffQTY { get; set; }

        /// <summary>
        /// 盘盈/盘亏
        /// </summary>
        [Display(Name = "ProfitLoss")]
        public string ProfitLoss { get; set; }

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

    public class IvActualCheckDetailExtend : IvActualCheckDetail
    {
        public string StockName { get; set; }
        public string GoodsLocationName { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
    }
}