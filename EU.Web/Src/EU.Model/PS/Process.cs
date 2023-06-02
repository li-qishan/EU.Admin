/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcess.cs
*
*功 能： N / A
* 类 名： PsProcess
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/18 23:25:31 SimonHsiao 初版
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
    [Entity(TableCnName = "工序", TableName = "PsProcess")]
    public class Process : Base.PersistPoco
    {
        /// <summary>
        /// 工序编号
        /// </summary>
        [Display(Name = "ProcessNo")]
        public string ProcessNo { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [Display(Name = "ProcessName")]
        public string ProcessName { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        [Display(Name = "WorkShopId")]
        public Guid? WorkShopId { get; set; }

        /// <summary>
        /// 加工类型，自制/外协/质检
        /// </summary>
        [Display(Name = "MachiningType")]
        public string MachiningType { get; set; }

        /// <summary>
        /// 工序类型，机台加工/非机台加工
        /// </summary>
        [Display(Name = "ProcessType")]
        public string ProcessType { get; set; }

        /// <summary>
        /// 外协定价，工时/重量
        /// </summary>
        [Display(Name = "PricingType")]
        public string PricingType { get; set; }
    }
}