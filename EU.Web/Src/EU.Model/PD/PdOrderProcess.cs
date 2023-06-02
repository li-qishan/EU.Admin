/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOrderProcess.cs
*
*功 能： N / A
* 类 名： PdOrderProcess
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/13 17:07:28 SimonHsiao 初版
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
    /// 生产工单工艺路线
    /// </summary>
    [Entity(TableCnName = "生产工单工艺路线", TableName = "PdOrderProcess")]
    public class PdOrderProcess : Base.PersistPoco
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
        [Display(Name = "SingleWeight")]
        public decimal SingleWeight { get; set; }

        /// <summary>
        /// 加工天数
        /// </summary>
        [Display(Name = "ProcessingDays")]
        public int ProcessingDays { get; set; }

        /// <summary>
        /// 调机时间（分钟）
        /// </summary>
        [Display(Name = "SetupTime")]
        public int SetupTime { get; set; }

        /// <summary>
        /// 标准工时，保留两位小数
        /// </summary>
        [Display(Name = "StandardHours")]
        public decimal StandardHours { get; set; }

        /// <summary>
        /// 工时单位，时分秒
        /// </summary>
        [Display(Name = "TimeUnit")]
        public string TimeUnit { get; set; }

        /// <summary>
        /// 标准工价
        /// </summary>
        [Display(Name = "StandardWages")]
        public decimal StandardWages { get; set; }

        /// <summary>
        /// 检验后转移
        /// </summary>
        [Display(Name = "IsTransfer")]
        public bool IsTransfer { get; set; }

        /// <summary>
        /// 工艺不良率（%），百分比数据
        /// </summary>
        [Display(Name = "RejectRate")]
        public decimal RejectRate { get; set; }
    }
}