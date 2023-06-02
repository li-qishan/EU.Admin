/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* ApPaymentOrder.cs
*
*功 能： N / A
* 类 名： ApPaymentOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/8 10:24:32 SimonHsiao 初版
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
    /// 采购付款单
    /// </summary>
    [Entity(TableCnName = "采购付款单", TableName = "ApPaymentOrder")]
    public class ApPaymentOrder : Base.PersistPoco
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
        /// 付款类型
        /// </summary>
        [Display(Name = "ApPaymentType")]
        public string ApPaymentType { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Display(Name = "SupplierId")]
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 冲销金额
        /// </summary>
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        [Display(Name = "ActualAmount")]
        public decimal? ActualAmount { get; set; }

        /// <summary>
        /// 收款差额
        /// </summary>
        [Display(Name = "DiffAmount")]
        public DateTime? DiffAmount { get; set; }
    }
}