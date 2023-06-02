/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmFieldCatalog.cs
*
*功 能： N / A
* 类 名： SmFieldCatalog
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/20 15:39:45 SimonHsiao 初版
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
    [Entity(TableCnName = "系统表栏位", TableName = "SmFieldCatalog")]
    public class SmFieldCatalog : Base.PersistPoco
    {


        /// <summary>
        /// 表代码
        /// </summary>
        [Display(Name = "TableCode")]
        public string TableCode { get; set; }

        /// <summary>
        /// 栏位代码
        /// </summary>
        [Display(Name = "ColumnCode")]
        public string ColumnCode { get; set; }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Display(Name = "ColumnName")]
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Display(Name = "DataType")]
        public string DataType { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        [Display(Name = "DataLength")]
        public int DataLength { get; set; }
    }
}