/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ArInvoiceOrder.cs
*
*功 能： N / A
* 类 名： ArInvoiceOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 11:22:29 SimonHsiao 初版
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
    /// 应收开票
    /// </summary>
    [Entity(TableCnName = "应收开票", TableName = "ArInvoiceOrder")]
    public class ArInvoiceOrder : Base.PersistPoco
    {

        /// <summary>
        /// 单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "ArInvoicetOrderType")]
        public string ArInvoicetOrderType { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [Display(Name = "CustomerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 未税金额
        /// </summary>
        [Display(Name = "NoTaxAmount")]
        public decimal? NoTaxAmount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [Display(Name = "TaxAmount")]
        public decimal? TaxAmount { get; set; }

        /// <summary>
        /// 含税金额
        /// </summary>
        [Display(Name = "TaxIncludedAmount")]
        public decimal? TaxIncludedAmount { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        [Display(Name = "InvoiceAmount")]
        public decimal? InvoiceAmount { get; set; }

        /// <summary>
        /// 发票差额
        /// </summary>
        [Display(Name = "InvoiceDiffAmount")]
        public decimal? InvoiceDiffAmount { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        [Display(Name = "CollectionTime")]
        public DateTime? CollectionTime { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "ArInvoicetOrderStatus")]
        public string ArInvoicetOrderStatus { get; set; }
    }
}