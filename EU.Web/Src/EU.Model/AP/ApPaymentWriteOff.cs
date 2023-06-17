/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ApPaymentWriteOff.cs
*
*功 能： N / A
* 类 名： ApPaymentWriteOff
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 10:26:28 SimonHsiao 初版
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
    /// 采购付款核销明细
    /// </summary>
    [Entity(TableCnName = "采购付款核销明细", TableName = "ApPaymentWriteOff")]
    public class ApPaymentWriteOff : Base.PersistPoco
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
        /// 发票号
        /// </summary>
        [Display(Name = "InvoiceNo")]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 开票日期
        /// </summary>
        [Display(Name = "InvoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [Display(Name = "CurrencyId")]
        public Guid? CurrencyId { get; set; }

        /// <summary>
        /// 发票税率
        /// </summary>
        [Display(Name = "InvoiceRate"), Column(TypeName = "decimal(20,2)")]
        public decimal? InvoiceRate { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        [Display(Name = "InvoiceAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? InvoiceAmount { get; set; }

        /// <summary>
        /// 预付金额
        /// </summary>
        [Display(Name = "PaymentAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// 未付金额
        /// </summary>
        [Display(Name = "NoPaymentAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? NoPaymentAmount { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        [Display(Name = "ActualPaymentAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? ActualPaymentAmount { get; set; }

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