/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdOrder.cs
*
*功 能： N / A
* 类 名： SdOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/26 15:44:13 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
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
    /// 销售单
    /// </summary>
    [Entity(TableCnName = "销售单", TableName = "SdOrder")]
    public class Order : Base.PersistPoco
    {


        /// <summary>
        /// 销售单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单日期/订购日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [Display(Name = "DeliveryrDate")]
        public DateTime? DeliveryrDate { get; set; }

        /// <summary>
        /// 订单类别 正式订单、样品订单、其他订单（默认正式订单）
        /// </summary>
        [Display(Name = "OrderCategory")]
        public string OrderCategory { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [Display(Name = "CustomerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 客户单号
        /// </summary>
        [Display(Name = "CustomerOrderNo")]
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        [Display(Name = "SalesmanId")]
        public Guid? SalesmanId { get; set; }

        /// <summary>
        /// 税别，参数值：TaxType
        /// </summary>
        [Display(Name = "TaxType")]
        public string TaxType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Display(Name = "TaxRate"), Column(TypeName = "decimal(20,6)")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// 未税金额
        /// </summary>
        [Display(Name = "NoTaxAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal NoTaxAmount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [Display(Name = "TaxAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// 含税金额
        /// </summary>
        [Display(Name = "TaxIncludedAmount"), Column(TypeName = "decimal(20,2)")]
        public decimal TaxIncludedAmount { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        [Display(Name = "SettlementWayId")]
        public Guid? SettlementWayId { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        [Display(Name = "SettlementWay")]
        public string SettlementWay { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [Display(Name = "CurrencyId")]
        public Guid? CurrencyId { get; set; }
    }
}