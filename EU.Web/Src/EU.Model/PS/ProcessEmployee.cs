/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcessEmployee.cs
*
*功 能： N / A
* 类 名： PsProcessEmployee
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/21 23:28:22 SimonHsiao 初版
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
    [Entity(TableCnName = "工序人员", TableName = "PsProcessEmployee")]
    public class ProcessEmployee : Base.PersistPoco
    {

        /// <summary>
        /// 工序单价ID
        /// </summary>
        [Display(Name = "ProcessId")]
        public Guid? ProcessId { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        [Display(Name = "EmployeeId")]
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// 机台ID
        /// </summary>
        [Display(Name = "MachineId")]
        public Guid? MachineId { get; set; }

        /// <summary>
        /// 标准单价
        /// </summary>
        [Display(Name = "Position")]
        public string Position { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [Display(Name = "Contact")]
        public string Contact { get; set; }
    }
}