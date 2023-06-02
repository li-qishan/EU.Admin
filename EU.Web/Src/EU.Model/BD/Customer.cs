/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdCustomer.cs
*
*功 能： N / A
* 类 名： BdCustomer
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/13 21:26:08 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Customer : Base.PersistPoco
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        [Display(Name = "客户编号")]
        public string CustomerNo { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "客户名称")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户等级
        /// </summary>
        [Display(Name = "客户等级")]
        public string CustomerLevel { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Display(Name = "联系人")]
        public string Contact { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 税别，参数值：TaxType
        /// </summary>
        [Display(Name = "税别，参数值：TaxType")]
        public string TaxType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Display(Name = "税率")]
        public decimal? TaxRate { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        [Display(Name = "结算方式")]
        public Guid? SettlementWayId { get; set; }

        /// <summary>
        /// 送货方式
        /// </summary>
        [Display(Name = "送货方式")]
        public Guid? DeliveryWayId { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [Display(Name = "币别")]
        public Guid? CurrencyId { get; set; }

        /// <summary>
        /// 信用额度
        /// </summary>
        [Display(Name = "信用额度")]
        public decimal? CreditLine { get; set; }

        /// <summary>
        /// 主营产品
        /// </summary>
        [Display(Name = "主营产品")]
        public string MainProduct { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        [Display(Name = "业务员")]
        public Guid? EmployeeId { get; set; }
    }
}