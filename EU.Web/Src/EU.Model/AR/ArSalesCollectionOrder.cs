/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ArSalesCollectionOrder.cs
*
*功 能： N / A
* 类 名： ArSalesCollectionOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 11:37:34 SimonHsiao 初版
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
    /// 销售收款单
    /// </summary>
    [Entity(TableCnName = "销售收款单", TableName = "ArSalesCollectionOrder")]
    public class ArSalesCollectionOrder : Base.PersistPoco
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
        /// 客户ID
        /// </summary>
        [Display(Name = "CustomerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        [Display(Name = "ReceivableAmount")]
        public decimal? ReceivableAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        [Display(Name = "ActualAmount")]
        public decimal? ActualAmount { get; set; }

        /// <summary>
        /// 收款差额
        /// </summary>
        [Display(Name = "DiffAmount")]
        public decimal? DiffAmount { get; set; }
    }
}