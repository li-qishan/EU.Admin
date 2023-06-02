/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdRequireAnalysis.cs
*
*功 能： N / A
* 类 名： PdRequireAnalysis
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/6 14:59:35 SimonHsiao 初版
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
    /// 工单分析
    /// </summary>
    [Entity(TableCnName = "工单分析", TableName = "PdRequireAnalysis")]
    public class PdRequireAnalysis : Base.PersistPoco
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
        /// 来源，销售单、生产计划单
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
        /// 订单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 项次
        /// </summary>
        [Display(Name = "SourceOrderSerialNumber")]
        public int SourceOrderSerialNumber { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        [Display(Name = "OrderQTY")]
        public decimal OrderQTY { get; set; }

        /// <summary>
        /// 增加比例
        /// </summary>
        [Display(Name = "AddRate")]
        public decimal AddRate { get; set; }

        /// <summary>
        /// 分析数量
        /// </summary>
        [Display(Name = "QTY")]
        public decimal QTY { get; set; }
    }
}