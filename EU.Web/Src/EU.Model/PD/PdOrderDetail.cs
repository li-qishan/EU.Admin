/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOrderDetail.cs
*
*功 能： N / A
* 类 名： PdOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/10 13:54:02 SimonHsiao 初版
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
    /// 生产工单 - 对应订单
    /// </summary>
    [Entity(TableCnName = "对应订单", TableName = "PdOrderDetail")]
    public class PdOrderDetail : Base.PersistPoco
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
        /// 来源单ID
        /// </summary>
        [Display(Name = "SourceOrderId")]
        public Guid? SourceOrderId { get; set; }

        /// <summary>
        /// 来源单明细ID
        /// </summary>
        [Display(Name = "SourceOrderDetailId")]
        public Guid? SourceOrderDetailId { get; set; }

        /// <summary>
        /// 来源项次
        /// </summary>
        [Display(Name = "SourceSerialNumber")]
        public decimal SourceSerialNumber { get; set; }

        /// <summary>
        /// 客户物料编码
        /// </summary>
        [Display(Name = "CustomerMaterialCode")]
        public string CustomerMaterialCode { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal QTY { get; set; }

        /// <summary>
        /// 订单交期
        /// </summary>
        [Display(Name = "DeliveryDate")]
        public DateTime? DeliveryDate { get; set; }
    }
}