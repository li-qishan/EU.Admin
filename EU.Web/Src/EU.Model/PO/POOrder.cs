/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PoOrder.cs
*
*功 能： N / A
* 类 名： PoOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/6 16:15:35 SimonHsiao 初版
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
    /// 采购单
    /// </summary>
    [Entity(TableCnName = "采购单", TableName = "PoOrder")]
    public class POOrder : Base.PersistPoco
    {


        /// <summary>
        /// 销售单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 作业日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 采购员ID
        /// </summary>
        [Display(Name = "UserId")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "DepartmentId")]
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// 采购类型，主材采购/耗材采购/样品采购/事物采购
        /// </summary>
        [Display(Name = "PurchaseType")]
        public string PurchaseType { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Display(Name = "SupplierId")]
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 供应商名字
        /// </summary>
        [Display(Name = "SupplierName")]
        public string SupplierName { get; set; }

        /// <summary>
        /// 预定交期
        /// </summary>
        [Display(Name = "ReserveDeliveryTime")]
        public DateTime? ReserveDeliveryTime { get; set; }

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
        /// 送货方式
        /// </summary>
        [Display(Name = "DeliveryWayId")]
        public Guid? DeliveryWayId { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        [Display(Name = "SettlementWayId")]
        public Guid? SettlementWayId { get; set; }

        /// <summary>
        /// 采购状态,未到货、部分到货、全部到货
        /// </summary>
        [Display(Name = "PoOrderStatus")]
        public string PoOrderStatus { get; set; }
    }
}