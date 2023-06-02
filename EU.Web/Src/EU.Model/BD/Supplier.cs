/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdSupplier.cs
*
*功 能： N / A
* 类 名： BdSupplier
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/5 13:55:39 SimonHsiao 初版
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
    [Entity(TableCnName = "供应商", TableName = "BdSupplier")]
    public class Supplier : Base.PersistPoco
    {


        /// <summary>
        /// 供应商编号
        /// </summary>
        [Display(Name = "SupplierNo")]
        public string SupplierNo { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Display(Name = "ShortName")]
        public string ShortName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 采购员
        /// </summary>
        [Display(Name = "BuyerId")]
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// 税别，参数值：TaxType
        /// </summary>
        [Display(Name = "TaxType")]
        public string TaxType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Display(Name = "TaxRate")]
        public decimal? TaxRate { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        [Display(Name = "SettlementWayId")]
        public Guid? SettlementWayId { get; set; }

        /// <summary>
        /// 送货方式
        /// </summary>
        [Display(Name = "DeliveryWayId")]
        public Guid? DeliveryWayId { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [Display(Name = "CurrencyId")]
        public Guid? CurrencyId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}