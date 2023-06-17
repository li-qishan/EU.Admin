/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcessTemplateDetail.cs
*
*功 能： N / A
* 类 名： PsProcessTemplateDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/25 15:42:09 SimonHsiao 初版
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
    [Entity(TableCnName = "工序模板明细", TableName = "PsProcessTemplateDetail")]
    public class ProcessTemplateDetail : Base.PersistPoco
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [Display(Name = "TemplateId")]
        public Guid? TemplateId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        [Display(Name = "ProcessId")]
        public Guid? ProcessId { get; set; }

        /// <summary>
        /// 机台ID
        /// </summary>
        [Display(Name = "MachineId")]
        public Guid? MachineId { get; set; }

        /// <summary>
        /// 重量单位，g/kg
        /// </summary>
        [Display(Name = "WeightUnit")]
        public string WeightUnit { get; set; }

        /// <summary>
        /// 单重
        /// </summary>
        [Display(Name = "PieceWeight"), Column(TypeName = "decimal(20,3)")]
        public decimal? PieceWeight { get; set; }

        /// <summary>
        /// 加工天数
        /// </summary>
        [Display(Name = "ProcessingDays")]
        public int ProcessingDays { get; set; }

        /// <summary>
        /// 调机时间（分钟）
        /// </summary>
        [Display(Name = "SetupTime"), Column(TypeName = "decimal(20,2)")]
        public decimal? SetupTime { get; set; }

        /// <summary>
        /// 标准工时，保留两位小数
        /// </summary>
        [Display(Name = "StandardHours"), Column(TypeName = "decimal(20,2)")]
        public decimal? StandardHours { get; set; }

        /// <summary>
        /// 工时单位，时分秒
        /// </summary>
        [Display(Name = "TimeUnit")]
        public string TimeUnit { get; set; }

        /// <summary>
        /// 标准工价
        /// </summary>
        [Display(Name = "StandardWages"), Column(TypeName = "decimal(20,2)")]
        public decimal? StandardWages { get; set; }

        /// <summary>
        /// 检验后转移
        /// </summary>
        [Display(Name = "IsTransfer")]
        public bool IsTransfer { get; set; }

        /// <summary>
        /// 工艺不良率（%），百分比数据
        /// </summary>
        [Display(Name = "RejectRate")]
        public int RejectRate { get; set; }
    }
}