/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdReturnOrderDetail.cs
*
*功 能： N / A
* 类 名： SdReturnOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/4 13:23:42 SimonHsiao 初版
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
    [Entity(TableCnName = "退货单明细", TableName = "SdReturnOrderDetail")]
    public class ReturnOrderDetail : Base.PersistPoco
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
        public int SerialNumber { get; set; }

        /// <summary>
        /// 出货来源
        /// </summary>
        [Display(Name = "OrderSource")]
        public string OrderSource { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        [Display(Name = "OutOrderNo")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 出库订单ID
        /// </summary>
        [Display(Name = "OutOrderId")]
        public Guid? OutOrderId { get; set; }

        /// <summary>
        /// 出库订单明细ID
        /// </summary>
        [Display(Name = "OutOrderDetailId")]
        public Guid? OutOrderDetailId { get; set; }

        /// <summary>
        /// 销售单号
        /// </summary>
        [Display(Name = "SalesOrderNo")]
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// 销售订单ID
        /// </summary>
        [Display(Name = "SalesOrderId")]
        public Guid? SalesOrderId { get; set; }

        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        [Display(Name = "SalesOrderDetailId")]
        public Guid? SalesOrderDetailId { get; set; }

        /// <summary>
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 退货数量
        /// </summary>
        [Display(Name = "ReturnQTY")]
        public decimal? ReturnQTY { get; set; }

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
        /// 客户物料编码
        /// </summary>
        [Display(Name = "CustomerMaterialCode")]
        public string CustomerMaterialCode { get; set; }

        /// <summary>
        /// 批次号ID
        /// </summary>
        [Display(Name = "BatchId")]
        public Guid? BatchId { get; set; }

        /// <summary>
        /// 退回仓库ID
        /// </summary>
        [Display(Name = "StockId")]
        public Guid? StockId { get; set; }

        /// <summary>
        /// 退回货位ID
        /// </summary>
        [Display(Name = "GoodsLocationId")]
        public Guid? GoodsLocationId { get; set; }

        /// <summary>
        /// 退货日期
        /// </summary>
        [Display(Name = "ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// 退货状态--待退回、已退回
        /// </summary>
        [Display(Name = "ReturnStatus")]
        public string ReturnStatus { get; set; }

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

        /// <summary>
        /// 是否实物退货
        /// </summary>
        [Display(Name = "IsEntity")]
        public bool IsEntity { get; set; }
    }
}