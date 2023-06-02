/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmImportError.cs
*
*功 能： N / A
* 类 名： SmImportError
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/24 13:14:37 SimonHsiao 初版
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
    [Entity(TableCnName = "导入错误", TableName = "SmImportError")]
    public class SmImportError : Base.PersistPoco
    {


        /// <summary>
        /// 错误代码
        /// </summary>
        [Display(Name = "ErrorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误名称
        /// </summary>
        [Display(Name = "ErrorName")]
        public string ErrorName { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        [Display(Name = "ErrorType")]
        public string ErrorType { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Display(Name = "ModuleCode")]
        public string ModuleCode { get; set; }

        /// <summary>
        /// Sheet名
        /// </summary>
        [Display(Name = "SheetName")]
        public string SheetName { get; set; }
    }
}