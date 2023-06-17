/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ArInvoiceAssociation.cs
*
*功 能： N / A
* 类 名： ArInvoiceAssociation
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/12/8 15:42:18 SimonHsiao 初版
*
* Copyright(c) 2021 SUZHOU EU Corporation. All Rights Reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using EU.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model
{

    /// <summary>
    /// 应收开票对应发票
    /// </summary>
    [Entity(TableCnName = "应收开票对应发票", TableName = "ArInvoiceAssociation")]
    public class ArInvoiceAssociation : Base.PersistPoco
    {

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId"), Description("订单ID")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber"), Description("序号")]
        public int? SerialNumber { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        [Display(Name = "InvoiceNo"), Description("发票号码")]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 发票日期
        /// </summary>
        [Display(Name = "InvoiceDate"), Description("发票日期")]
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// 发票税率
        /// </summary>
        [Display(Name = "TaxRate"), Description("发票税率"), Column(TypeName = "decimal(20,2)")]
        public decimal? TaxRate { get; set; }

        /// <summary>
        /// 未税金额
        /// </summary>
        [Display(Name = "NoTaxAmount"), Description("未税金额"), Column(TypeName = "decimal(20,2)")]
        public decimal? NoTaxAmount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [Display(Name = "TaxAmount"), Description("税额"), Column(TypeName = "decimal(20,2)")]
        public decimal? TaxAmount { get; set; }

        /// <summary>
        /// 含税金额
        /// </summary>
        [Display(Name = "TaxIncludedAmount"), Description("含税金额"), Column(TypeName = "decimal(20,2)")]
        public decimal? TaxIncludedAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark1"), Description("备注")]
        public string ExtRemark1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark2"), Description("备注")]
        public string ExtRemark2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark3"), Description("备注")]
        public string ExtRemark3 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark4"), Description("备注")]
        public string ExtRemark4 { get; set; }
    }
}