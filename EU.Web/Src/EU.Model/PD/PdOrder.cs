/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOrder.cs
*
*功 能： N / A
* 类 名： PdOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/6/13 20:20:30 SimonHsiao 初版
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
    /// 生产工单
    /// </summary>
    [Entity(TableCnName = "生产工单", TableName = "PdOrder")]
    public class PdOrder : Base.PersistPoco
    {

        /// <summary>
        /// 单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 作业日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 货品编号
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal QTY { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Display(Name = "Source")]
        public string Source { get; set; }

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
        /// 来源单号
        /// </summary>
        [Display(Name = "SourceOrderNo")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        [Display(Name = "UrgentType")]
        public string UrgentType { get; set; }

        /// <summary>
        /// 已完工数量
        /// </summary>
        [Display(Name = "CompleteQTY")]
        public decimal CompleteQTY { get; set; }

        /// <summary>
        /// 预开工日
        /// </summary>
        [Display(Name = "PlanStartDate")]
        public DateTime? PlanStartDate { get; set; }

        /// <summary>
        /// 预完工日
        /// </summary>
        [Display(Name = "PlanEndDate")]
        public DateTime? PlanEndDate { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        [Display(Name = "WorkShopId")]
        public Guid? WorkShopId { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        [Display(Name = "RequireDate")]
        public DateTime? RequireDate { get; set; }
    }
}