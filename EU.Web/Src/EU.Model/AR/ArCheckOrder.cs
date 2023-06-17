/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ArStatementOrder.cs
*
*功 能： N / A
* 类 名： ArStatementOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 10:39:09 SimonHsiao 初版
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
    /// 应收对账单
    /// </summary>
    [Entity(TableCnName = "应收对账单", TableName = "ArStatementOrder")]
    public class ArCheckOrder : Base.PersistPoco
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
        /// 供应商ID
        /// </summary>
        [Display(Name = "SupplierId")]
        public Guid? SupplierId { get; set; }

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
        /// 周期开始时间
        /// </summary>
        [Display(Name = "PeriodBeginTime")]
        public DateTime? PeriodBeginTime { get; set; }

        /// <summary>
        /// 周期结束时间
        /// </summary>
        [Display(Name = "PeriodEndTime")]
        public DateTime? PeriodEndTime { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "StatementOrderStatus")]
        public string StatementOrderStatus { get; set; }
    }
}