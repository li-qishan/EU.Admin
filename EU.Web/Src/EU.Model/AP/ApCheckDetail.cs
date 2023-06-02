/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ApCheckDetail.cs
*
*功 能： N / A
* 类 名： ApCheckDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/7 20:25:01 SimonHsiao 初版
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

    /// <summary>
    /// 应付对账单明细
    /// </summary>
    [Entity(TableCnName = "应付对账单明细", TableName = "ApCheckDetail")]
    public class ApCheckDetail : Base.PersistPoco
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
        public int? SerialNumber { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        [Display(Name = "OrderSource")]
        public string OrderSource { get; set; }

        /// <summary>
        /// 来源单ID
        /// </summary>
        [Display(Name = "SourceOrderId")]
        public Guid? SourceOrderId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        [Display(Name = "SourceOrderNo")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 来源单日期
        /// </summary>
        [Display(Name = "SourceOrderDate")]
        public DateTime? SourceOrderDate { get; set; }

        /// <summary>
        /// 来源单明细ID
        /// </summary>
        [Display(Name = "SourceOrderDetailSerialId")]
        public Guid? SourceOrderDetailId { get; set; }

        /// <summary>
        /// 来源单序号
        /// </summary>
        [Display(Name = "SourceOrderDetailSerialNumber")]
        public int? SourceOrderDetailSerialNumber { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 交易数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal? QTY { get; set; }

        ///// <summary>
        ///// 已对账数量
        ///// </summary>
        //[Display(Name = "HasCheckQTY")]
        //public decimal? HasCheckQTY { get; set; }

        ///// <summary>
        ///// 未对账数量
        ///// </summary>
        //[Display(Name = "NoCheckQTY")]
        //public decimal? NoCheckQTY { get; set; }

        /// <summary>
        /// 对账数量
        /// </summary>
        [Display(Name = "CheckQTY")]
        public decimal? CheckQTY { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Display(Name = "TaxRate")]
        public decimal? TaxRate { get; set; }

        /// <summary>
        /// 未税金额
        /// </summary>
        [Display(Name = "NoTaxAmount")]
        public decimal? NoTaxAmount { get; set; }

        /// <summary>
        /// 含税金额
        /// </summary>
        [Display(Name = "TaxIncludedAmount")]
        public decimal? TaxIncludedAmount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [Display(Name = "TaxAmount")]
        public decimal? TaxAmount { get; set; }

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