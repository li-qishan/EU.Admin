/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdPlanDetail.cs
*
*功 能： N / A
* 类 名： PdPlanDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/8/15 17:11:55 SimonHsiao 初版
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
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model
{

    /// <summary>
    /// 生产计划工单明细
    /// </summary>
    [Entity(TableCnName = "", TableName = "PdPlanDetail")]
    public class PdPlanDetail : Base.PersistPoco
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
        public int? SerialNumber { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 补发数量
        /// </summary>
        [Display(Name = "QTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? QTY { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        [Display(Name = "RequireDate")]
        public DateTime? RequireDate { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        [Display(Name = "BOMId")]
        public Guid? BOMId { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        [Display(Name = "CompleteQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? CompleteQTY { get; set; }

        /// <summary>
        /// 转生产量
        /// </summary>
        [Display(Name = "TransferQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? TransferQTY { get; set; }

        /// <summary>
        /// 客户物料编号
        /// </summary>
        [Display(Name = "CustomerMaterialNo")]
        public string CustomerMaterialNo { get; set; }

        /// <summary>
        /// 客户物料名称
        /// </summary>
        [Display(Name = "CustomerMaterialName")]
        public string CustomerMaterialName { get; set; }
    }
}