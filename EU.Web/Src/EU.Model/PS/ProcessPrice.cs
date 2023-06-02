/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcessPrice.cs
*
*功 能： N / A
* 类 名： PsProcessPrice
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/21 22:44:44 SimonHsiao 初版
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
    [Entity(TableCnName = "工序单价", TableName = "PsProcessPrice")]
    public class ProcessPrice : Base.PersistPoco
    {
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
        /// 标准单价
        /// </summary>
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 时间单位，小时/分钟/秒，从机台资料带出来，如果机台资料为空，可手动选择时分秒
        /// </summary>
        [Display(Name = "TimeUnit")]
        public string TimeUnit { get; set; }
    }
}