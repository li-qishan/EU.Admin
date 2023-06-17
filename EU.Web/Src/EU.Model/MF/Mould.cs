/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* MfMould.cs
*
*功 能： N / A
* 类 名： MfMould
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/6/7 21:43:19 SimonHsiao 初版
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
    [Entity(TableCnName = "工模治具", TableName = "MfMould")]
    public class Mould : Base.PersistPoco
    {
        /// <summary>
        /// 工模治具编号
        /// </summary>
        [Display(Name = "MouldNo")]
        public string MouldNo { get; set; }

        /// <summary>
        /// 工模治具名称
        /// </summary>
        [Display(Name = "MouldName")]
        public string MouldName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// 材质ID
        /// </summary>
        [Display(Name = "TextureId")]
        public Guid? TextureId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }

        /// <summary>
        /// 工模治具类别ID
        /// </summary>
        [Display(Name = "MouldTypeId")]
        public Guid? MouldTypeId { get; set; }

        /// <summary>
        /// 模穴数
        /// </summary>
        [Display(Name = "QTY"), Column(TypeName = "decimal(20,2)")]
        public int QTY { get; set; }

        /// <summary>
        /// 成型时间（S）
        /// </summary>
        [Display(Name = "MoldingTime"), Column(TypeName = "decimal(20,2)")]
        public decimal MoldingTime { get; set; }

        /// <summary>
        /// 现有数量
        /// </summary>
        [Display(Name = "CurrentQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal CurrentQTY { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        [Display(Name = "AvailableQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal AvailableQTY { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "MouldDesc")]
        public string MouldDesc { get; set; }
    }
}