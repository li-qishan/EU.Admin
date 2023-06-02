/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* EmMachineType.cs
*
*功 能： N / A
* 类 名： EmMachineType
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/16 21:21:24 SimonHsiao 初版
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
    [Entity(TableCnName = "设备分类", TableName = "EmMachineType")]
    public class MachineType : Base.PersistPoco
    {


        /// <summary>
        /// 分类编号
        /// </summary>
        [Display(Name = "TypeNo")]
        public string TypeNo { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Display(Name = "TypeName")]
        public string TypeName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "DepartmentId")]
        public Guid? DepartmentId { get; set; }
    }
}