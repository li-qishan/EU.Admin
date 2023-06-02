/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcessBadReason.cs
*
*功 能： N / A
* 类 名： PsProcessBadReason
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/22 12:07:01 SimonHsiao 初版
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
    [Entity(TableCnName = "工序不良原因", TableName = "PsProcessBadReason")]
    public class ProcessBadReason : Base.PersistPoco
    {
        /// <summary>
        /// 工序单价ID
        /// </summary>
        [Display(Name = "ProcessId")]
        public Guid? ProcessId { get; set; }

        /// <summary>
        /// 不良原因代码
        /// </summary>
        [Display(Name = "BadCode")]
        public string BadCode { get; set; }

        /// <summary>
        /// 不良类型
        /// </summary>
        [Display(Name = "ProcessBadType")]
        public string ProcessBadType { get; set; }

        /// <summary>
        /// 不良描述
        /// </summary>
        [Display(Name = "BadDesc")]
        public string BadDesc { get; set; }
    }
}