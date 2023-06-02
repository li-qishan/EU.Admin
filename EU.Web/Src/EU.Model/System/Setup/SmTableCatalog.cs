/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmTableCatalog.cs
*
*功 能： N / A
* 类 名： SmTableCatalog
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/20 15:34:03 SimonHsiao 初版
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
    /// 系统表字典
    /// </summary>
    [Entity(TableCnName = "系统表字典", TableName = "SmTableCatalog")]
    public class SmTableCatalog : Base.PersistPoco
    {


        /// <summary>
        /// 表代码
        /// </summary>
        [Display(Name = "TableCode")]
        public string TableCode { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        [Display(Name = "TableName")]
        public string TableName { get; set; }

        /// <summary>
        /// 表类型
        /// </summary>
        [Display(Name = "TypeCode")]
        public string TypeCode { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "TaxisNo")]
        public int TaxisNo { get; set; }
    }
}