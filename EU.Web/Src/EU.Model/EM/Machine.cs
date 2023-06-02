/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* EmMachine.cs
*
*功 能： N / A
* 类 名： EmMachine
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/17 22:07:48 SimonHsiao 初版
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
    [Entity(TableCnName = "设备基础资料", TableName = "EmMachine")]
    public class Machine : Base.PersistPoco
    {


        /// <summary>
        /// 设备编号
        /// </summary>
        [Display(Name = "MachineNo")]
        public string MachineNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Display(Name = "MachineName")]
        public string MachineName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [Display(Name = "MachineType")]
        public Guid? MachineType { get; set; }

        /// <summary>
        /// 设备状态,加工中/空闲中/报修中
        /// </summary>
        [Display(Name = "MachineStatus")]
        public string MachineStatus { get; set; }

        /// <summary>
        /// 标准加工时间
        /// </summary>
        [Display(Name = "StandardMachineTime")]
        public decimal? StandardMachineTime { get; set; }

        /// <summary>
        /// 最大加工时间
        /// </summary>
        [Display(Name = "MaxMachineTime")]
        public decimal? MaxMachineTime { get; set; }

        /// <summary>
        /// 时间单位，小时/分钟/秒
        /// </summary>
        [Display(Name = "TimeUnit")]
        public string TimeUnit { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [Display(Name = "Location")]
        public string Location { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        [Display(Name = "ResponsibleId")]
        public Guid? ResponsibleId { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Display(Name = "FileCode")]
        public Guid? FileCode { get; set; }
    }
}