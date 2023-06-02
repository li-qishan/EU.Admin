/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ApPaymentDetail.cs
*
*功 能： N / A
* 类 名： ApPaymentDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 10:18:32 SimonHsiao 初版
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
    /// 采购付款单明细
    /// </summary>
    [Entity(TableCnName = "采购付款单明细", TableName = "ApPaymentDetail")]
    public class ApPaymentDetail : Base.PersistPoco
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
        /// 付款类型（转账、支票、承兑、现金、其他）现金、其他）
        /// </summary>
        [Display(Name = "PaymentType")]
        public string PaymentType { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        [Display(Name = "PaymentAmount")]
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        [Display(Name = "PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "BankName")]
        public string BankName { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        [Display(Name = "InvoiceNo")]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        [Display(Name = "DueDate")]
        public DateTime? DueDate { get; set; }

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