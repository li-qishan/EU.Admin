/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ArStatementDetail.cs
*
*功 能： N / A
* 类 名： ArStatementDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 10:39:44 SimonHsiao 初版
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
    /// 应收对账单明细
    /// </summary>
    [Entity(TableCnName = "应收对账单明细", TableName = "ArStatementDetail")]
    public class ArCheckDetail : Base.PersistPoco
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
        /// 来源订单ID
        /// </summary>
        [Display(Name = "SourceOrderId")]
        public Guid? SourceOrderId { get; set; }

        /// <summary>
        /// 来源订单号
        /// </summary>
        [Display(Name = "SourceOrderNo")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 销售单号
        /// </summary>
        [Display(Name = "SalesOrderNo")]
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// 来源单日期
        /// </summary>
        [Display(Name = "SourceOrderDate")]
        public DateTime? SourceOrderDate { get; set; }

        /// <summary>
        /// 出库订单明细ID
        /// </summary>
        [Display(Name = "SourceOrderDetailId")]
        public Guid? SourceOrderDetailId { get; set; }

        /// <summary>
        /// 来源订单明细序号
        /// </summary>
        [Display(Name = "SourceSerialNumber")]
        public int? SourceSerialNumber { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 交易数量
        /// </summary>
        [Display(Name = "TradeQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? TradeQTY { get; set; }

        /// <summary>
        /// 已对账数量
        /// </summary>
        [Display(Name = "HasCheckQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? HasCheckQTY { get; set; }

        /// <summary>
        /// 未对账数量
        /// </summary>
        [Display(Name = "NoCheckQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? NoCheckQTY { get; set; }

        /// <summary>
        /// 对账数量
        /// </summary>
        [Display(Name = "CheckQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? CheckQTY { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "Price"), Column(TypeName = "decimal(20,2)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Display(Name = "TaxRate"), Column(TypeName = "decimal(20,6)")]
        public decimal? TaxRate { get; set; }

        /// <summary>
        /// 未税金额
        /// </summary>
        [Display(Name = "NoTaxAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? NoTaxAmount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [Display(Name = "TaxAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? TaxAmount { get; set; }

        /// <summary>
        /// 含税金额
        /// </summary>
        [Display(Name = "TaxIncludedAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal? TaxIncludedAmount { get; set; }

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